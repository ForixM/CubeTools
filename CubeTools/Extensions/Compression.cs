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
        public static void CompressFiles(string[] files, string dest, CompressAlgo algo)
        {
            if (algo == CompressAlgo.LZMA)
            {
                SevenZipCompressor compressor = new SevenZipCompressor();
                compressor.ScanOnlyWritable = true;
                compressor.CompressFiles(dest, files);
                return;
            }

            if (algo == CompressAlgo.ZIP)
            {
                using (ZipArchive zip = ZipFile.Open(dest, ZipArchiveMode.Create))
                {
                    foreach (string file in files)
                    {
                        zip.CreateEntryFromFile(file, file);
                    }
                    
                }
            }
        }
    }
}