using System.Collections.Generic;

namespace CubeTools
{
    public class Compression
    {
        public static void CompressFiles(string[] files, string dest)
        {
            
            compressor.ScanOnlyWritable = true;
            compressor.CompressFiles(dest, files);
        }
    }
}