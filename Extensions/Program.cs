using System;
using System.Collections.Generic;
using System.IO;
using CubeTools;
using SevenZip;

namespace Extensions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            //Compression.Init();
            //Compression.CompressDirectory("C:/Test/", "C:/Zipped/arch.7z", CompressAlgo.LZMA);
            FtpUtils ftp = new FtpUtils("127.0.0.1", "forix", "jsp2003");
            List<string> files = ftp.GetFiles();
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            ftp.MoveDir("Oklm");
            files = ftp.GetFiles();
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
        }
    }
}
