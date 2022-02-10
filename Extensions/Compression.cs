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
    public enum CompressAlgo
    {
        ZIP,
        LZMA
    }

    public class Compression
    {
        public static void Init()
        {
            var path = @"C:\Users\forix\Documents\CubeTools\Extensions\bin\Debug\net5.0\Assets\7z.dll";
            SevenZipBase.SetLibraryPath(path);
            Console.WriteLine(SevenZipBase.CurrentLibraryFeatures);
        }

        public static Task CompressDirectory(DirectoryType directory, FileType dest, CompressAlgo algo)
        {
            switch (algo)
            {
                case CompressAlgo.ZIP:
                    Action<object> action = (object obj) => { ZipFile.CreateFromDirectory(directory.Path, dest.Path); };
                    Task task = new Task(action, "compression");
                    task.Start();
                    return task;
                case CompressAlgo.LZMA:
                    SevenZipCompressor compressor = new SevenZipCompressor();
                    compressor.ScanOnlyWritable = true;
                    //compressor.CompressDirectory(directory.Path, dest.Path);
                    return compressor.CompressDirectoryAsync(directory.Path, dest.Path);
            }

            return null;
        }

        public static Task CompressFiles(FileType[] files, FileType dest, CompressAlgo algo)
        {
            switch (algo)
            {
                case CompressAlgo.ZIP:
                    Action<object> action = (object obj) =>
                    {
                        using (ZipArchive zip = ZipFile.Open(dest.Path, ZipArchiveMode.Create))
                        {
                            foreach (FileType file in files)
                            {
                                zip.CreateEntryFromFile(file.Path, file.Name);
                            }
                        }
                    };
                    Task task = new Task(action, "compression");
                    task.Start();
                    return task;
                    break;
                case CompressAlgo.LZMA:
                    SevenZipCompressor compressor = new SevenZipCompressor();
                    compressor.ScanOnlyWritable = true;
                    string[] filePaths = new string[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        filePaths[i] = files[i].Path;
                    }

                    return compressor.CompressFilesAsync(dest.Path, filePaths);
            }

            return null;
        }
    }
}