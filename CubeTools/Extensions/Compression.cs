using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
        public static void CompressDirectory(string directory, string dest, CompressAlgo algo)
        {
            switch (algo)
            {
                case CompressAlgo.ZIP:
                    ZipFile.CreateFromDirectory(directory, dest);
                    break;
                case CompressAlgo.LZMA:
                    SevenZipCompressor compressor = new SevenZipCompressor();
                    compressor.ScanOnlyWritable = true;
                    compressor.CompressDirectory(directory, dest);
                    break;
            }
        }

        public static void CompressFiles(string[] files, string dest, CompressAlgo algo)
        {
            switch (algo)
            {
                case CompressAlgo.ZIP:
                    using (ZipArchive zip = ZipFile.Open(dest, ZipArchiveMode.Create))
                    {
                        foreach (string file in files)
                        {
                            FileInfo info = new FileInfo(file);
                            zip.CreateEntryFromFile(file, info.Name);
                        }
                    }
                    break;
                case CompressAlgo.LZMA:
                    SevenZipCompressor compressor = new SevenZipCompressor();
                    compressor.ScanOnlyWritable = true;
                    compressor.CompressFiles(dest, files);
                    break;
            }
        }
    }
}