﻿using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerReader;
using SevenZip;

namespace LibraryCompression
{
    public class Compression
    {
        private static bool _initialized;

        /// <summary>
        ///     Initializer for compression/decompression functionalities
        /// </summary>
        public static void Init()
        {
            if (ConfigLoader.ConfigLoader.AppPath is "" or ".") SevenZipBase.SetLibraryPath("7z.dll");
            else if (ConfigLoader.ConfigLoader.AppPath![^1] == '/') SevenZipBase.SetLibraryPath(ConfigLoader.ConfigLoader.AppPath + "7z.dll");
            else SevenZipBase.SetLibraryPath(ConfigLoader.ConfigLoader.AppPath + "/7z.dll");

            Console.WriteLine(SevenZipBase.CurrentLibraryFeatures);
            _initialized = true;
        }

        /// <summary>
        ///     Compress the given directory and all his sub-contents into a arvhie file with the same name that the
        ///     directory name in the same path of the directory.
        /// </summary>
        /// <param name="directoryLocal">Directory to compress</param>
        /// <param name="archiveFormat">Compression algorithm to use. Only ZIP and SevenZip are supported</param>
        /// <param name="compressingEvent">Optional method parameter to track compression progression</param>
        /// <param name="finishedEvent">Optional method parameter called when compression have finished</param>
        /// <returns>Compression Task which is async and already ran</returns>
        public static Task CompressDirectory(DirectoryLocalPointer directoryLocal, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs> compressingEvent = null, Action<object, EventArgs> finishedEvent = null)
        {
            if (!_initialized)
                throw new SystemException("Compression haven't been initialized");
            var dest = directoryLocal.Path[^1] == '/'
                ? directoryLocal.Path.Remove(directoryLocal.Path.Length - 1, 1)
                : directoryLocal.Path;
            switch (archiveFormat)
            {
                case OutArchiveFormat.SevenZip:
                    dest += ".7z";
                    break;
                case OutArchiveFormat.Zip:
                    dest += ".zip";
                    break;
                default:
                    throw new ArgumentException("Archive format not supported: " + archiveFormat);
            }
            return CompressDirectory(directoryLocal, dest, archiveFormat, compressingEvent, finishedEvent);
        }

        /// <summary>
        ///     Compress the given directory and all his sub-contents into the given destination
        /// </summary>
        /// <param name="directoryLocal">Directory to compress</param>
        /// <param name="dest">File where the directory will be compressed</param>
        /// <param name="archiveFormat">Compression algorithm to use. Only ZIP and SevenZip are supported</param>
        /// <param name="compressingEvent">Optional method parameter to track compression progression</param>
        /// <param name="finishedEvent">Optional method parameter called when compression have finished</param>
        /// <returns>Compression Task which is async and already ran</returns>
        public static Task CompressDirectory(DirectoryLocalPointer directoryLocal, string dest = "", OutArchiveFormat archiveFormat = OutArchiveFormat.SevenZip,
            Action<object, ProgressEventArgs>? compressingEvent = null, Action<object, EventArgs>? finishedEvent = null)
        {
            if (!_initialized)
                throw new SystemException("Compression haven't been initialized");
            // Initialization of compressor
            var compressor = new SevenZipCompressor()
            {
                ArchiveFormat = archiveFormat,
                ScanOnlyWritable = true
            };
            // Adding parameters
            if (compressingEvent != null) compressor.Compressing += new EventHandler<ProgressEventArgs>(compressingEvent!);
            if (finishedEvent != null) compressor.CompressionFinished += new EventHandler<EventArgs>(finishedEvent!);
            compressor.CustomParameters.Add("mt", "on");
            // Return compressor
            return compressor.CompressDirectoryAsync(directoryLocal.Path, dest);
        }

        /// <summary>
        ///     Compress the given files table into an archive file with the same name that the parent directory of the
        ///     first file in the table
        /// </summary>
        /// <param name="files">The FilePointer table to compress</param>
        /// <param name="archiveFormat">Compression algorithm to use. Only ZIP and SevenZip are supported</param>
        /// <param name="compressingEvent">Optional method parameter to track compression progression</param>
        /// <param name="finishedEvent">Optional method parameter called when compression have finished</param>
        /// <returns>Compression Task which is async and already ran</returns>
        public static Task CompressFiles(FileLocalPointer[] files, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs>? compressingEvent = null, Action<object, EventArgs>? finishedEvent = null)
         => CompressFiles(files, files[0].Path, archiveFormat, compressingEvent, finishedEvent);

        /// <summary>
        ///     Compress the given files table into the given archive destination
        /// </summary>
        /// <param name="files">The FilePointer table to compress</param>
        /// <param name="dest">The archive destination where all files will be compressed</param>
        /// <param name="archiveFormat">Compression algorithm to use. Only ZIP and SevenZip are supported</param>
        /// <param name="compressingEvent">Optional method parameter to track compression progression</param>
        /// <param name="finishedEvent">Optional method parameter called when compression have finished</param>
        /// <returns>Compression Task which is async and already ran</returns>
        public static Task CompressFiles(FileLocalPointer[] files, string dest, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs>? compressingEvent = null, Action<object, EventArgs>? finishedEvent = null)
        {
            if (!_initialized)
                throw new SystemException("Compression haven't been initialized");
            // Initialized compressor
            var compressor = new SevenZipCompressor();
            // Adding parameters
            if (compressingEvent != null) compressor.Compressing += new EventHandler<ProgressEventArgs>(compressingEvent!);
            if (finishedEvent != null) compressor.CompressionFinished += new EventHandler<EventArgs>(finishedEvent!);
            compressor.ArchiveFormat = archiveFormat;
            compressor.CustomParameters.Add("mt", "on");
            compressor.ScanOnlyWritable = true;
            // Adding files path
            var filePaths = new string[files.Length];
            for (var i = 0; i < files.Length; i++) filePaths[i] = files[i].Path;
            return compressor.CompressFilesAsync(dest, filePaths);
        }

        public static Task CompressItems(List<Pointer> directories, string dest, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs>? compressingEvent = null, Action<object, EventArgs>? finishedEvent = null)
        {
            if (!_initialized)
                throw new SystemException("Compression haven't been initialized");
            
            // Initialized compressor
            var compressor = new SevenZipCompressor()
            {
                ArchiveFormat = archiveFormat,
                ScanOnlyWritable = true,
                DirectoryStructure = true,
                PreserveDirectoryRoot = false
            };
            
            // Adding parameters
            if (compressingEvent != null) compressor.Compressing += new EventHandler<ProgressEventArgs>(compressingEvent!);
            if (finishedEvent != null) compressor.CompressionFinished += new EventHandler<EventArgs>(finishedEvent!);
            compressor.CustomParameters.Add("mt", "on");
            
            // Adding files path
            Dictionary<string, string> files = new Dictionary<string, string>();
            foreach (LocalPointer item in directories)
            {
                if (item.IsDir) DictionarizeFolder(files, item.Path, ManagerReader.GetParent(item.Path));
                else files.Add(item.Name, item.Path);
            }

            return new Task(() => compressor.CompressFileDictionary(files, dest));
        }
        
        private static void DictionarizeFolder(Dictionary<string,string> files, string dir, string supremeDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            foreach (FileInfo file in dirInfo.GetFiles())
            {
                files.Add(file.FullName.Replace(supremeDir.Replace("/", "\\")+"\\", ""),file.FullName);
            }
            foreach (DirectoryInfo directory in dirInfo.GetDirectories())
            {
                DictionarizeFolder(files, directory.FullName, supremeDir);
            }
        }

        /// <summary>
        ///     Extract all the content of the given archive to a directory with the same name of the archive in the same
        ///     path.
        /// </summary>
        /// <param name="archive">The archive path</param>
        /// <param name="compressingEvent">Optional method parameter to track extraction progression</param>
        /// <param name="finishedEvent">Optional method parameter called when extraction have finished</param>
        /// <returns>Extraction Task which is async and already ran</returns>
        public static Task Extract(LocalPointer archive, Action<object, ProgressEventArgs>? compressingEvent = null,
            Action<object, EventArgs>? finishedEvent = null)
        {
            var dest = archive.Path;
            while (dest[^1] != '.') dest = dest.Remove(dest.Length - 1, 1);
            dest = dest.Remove(dest.Length - 1, 1);
            dest = dest.Replace("/", @"\");
            if (Directory.Exists(dest) || File.Exists(dest))
                dest = ManagerReader.GenerateNameForModification(dest);
            return Extract(archive, dest, compressingEvent, finishedEvent);
        }

        /// <summary>
        ///     Extract all the content of the given archive to the given archive path
        /// </summary>
        /// <param name="archive">The archive path</param>
        /// <param name="destination">The archive destination path</param>
        /// <param name="compressingEvent">Optional method parameter to track extraction progression</param>
        /// <param name="finishedEvent">Optional method parameter called when extraction have finished</param>
        /// <returns>Extraction Task which is async and already ran</returns>
        public static Task Extract(LocalPointer archive, string destination,
            Action<object, ProgressEventArgs> compressingEvent = null, Action<object, EventArgs> finishedEvent = null)
        {
            destination = destination.Replace("/", @"\");
            var extractor = new SevenZipExtractor(archive.Path);
            if (compressingEvent != null) extractor.Extracting += new EventHandler<ProgressEventArgs>(compressingEvent);
            if (finishedEvent != null) extractor.ExtractionFinished += new EventHandler<EventArgs>(finishedEvent);
            return extractor.ExtractArchiveAsync(destination);
        }
    }
}