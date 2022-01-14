using System.Collections.Generic;
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
        public static void CompressFiles(string[] files, string dest)
        {
            SevenZipCompressor compressor = new SevenZipCompressor();
            compressor.ScanOnlyWritable = true;
            compressor.CompressFiles(dest, files);
        }
    }
}