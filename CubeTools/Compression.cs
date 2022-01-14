﻿using System.Collections.Generic;
using HarfBuzzSharp;
using SevenZip;

namespace CubeTools
{
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