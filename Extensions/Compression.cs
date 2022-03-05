using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Manager;
using SevenZip;

namespace CubeTools
{
    public class Compression
    {
        private static bool _initialized = false;
        public static void Init()
        {
            var path = @"C:\Users\forix\Documents\CubeTools\Extensions\bin\Debug\net5.0\Assets\7z.dll";
            SevenZipBase.SetLibraryPath(path);
            Console.WriteLine(SevenZipBase.CurrentLibraryFeatures);
            _initialized = true;
        }

        public static Task CompressDirectory(DirectoryType directory, FileType dest, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs> compressingEvent = null, Action<object, EventArgs> finishedEvent = null)
        {
            if (!_initialized)
                throw new SystemException("Compression haven't been initialized");
            SevenZipCompressor compressor = new SevenZipCompressor();
            if (compressingEvent != null)
                compressor.Compressing += new EventHandler<ProgressEventArgs>(compressingEvent);
            if (finishedEvent != null)
                compressor.CompressionFinished += new EventHandler<EventArgs>(finishedEvent);
            compressor.ArchiveFormat = archiveFormat;
            compressor.CustomParameters.Add("mt", "on");
            compressor.ScanOnlyWritable = true;
            return compressor.CompressDirectoryAsync(directory.Path, dest.Path);
        }

        public static Task CompressFiles(FileType[] files, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs> compressingEvent = null, Action<object, EventArgs> finishedEvent = null)
        {
            FileType dest = files[0];
            string parent = ManagerReader.GetParent(files[0]);
            string name = ManagerReader.GetPathToName(parent);
            string path = ManagerReader.GetParent(files[0]) + "/" + ManagerReader.GetPathToName(parent);
            switch (archiveFormat)
            {
                case OutArchiveFormat.SevenZip:
                    path += ".7z";
                    break;
                case OutArchiveFormat.Zip:
                    path += ".zip";
                    break;
                default:
                    throw new ArgumentException("Archive format not supported: " + archiveFormat);
            }
            
            return CompressFiles(files, path, archiveFormat, compressingEvent, finishedEvent);
        }
        
        public static Task CompressFiles(FileType[] files, string dest, OutArchiveFormat archiveFormat,
            Action<object, ProgressEventArgs> compressingEvent = null, Action<object, EventArgs> finishedEvent = null)
        {
            if (!_initialized)
                throw new SystemException("Compression haven't been initialized");
            SevenZipCompressor compressor = new SevenZipCompressor();
            if (compressingEvent != null)
                compressor.Compressing += new EventHandler<ProgressEventArgs>(compressingEvent);
            if (finishedEvent != null)
                compressor.CompressionFinished += new EventHandler<EventArgs>(finishedEvent);
            compressor.ArchiveFormat = archiveFormat;
            compressor.CustomParameters.Add("mt", "on");
            compressor.ScanOnlyWritable = true;
            string[] filePaths = new string[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                filePaths[i] = files[i].Path;
            }

            return compressor.CompressFilesAsync(dest, filePaths);
        }
    }
}