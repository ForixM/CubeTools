using System;
using System.Collections.Generic;
using System.IO;
using CubeTools;
using Manager;
using SevenZip;

namespace Extensions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Compression.Init();
            /*FileInfo filet = new FileInfo("Assets/7z.dll");
            Console.WriteLine(filet.FullName);*/

            Console.WriteLine("Compression en cours...");
            Compression.CompressDirectory(new DirectoryType("C:/Test/"), new FileType("C:/Zipped/arch.7z"), CompressAlgo.LZMA);
            Console.WriteLine("Compression effectué");
            /*FtpUtils ftp = new FtpUtils("127.0.0.1", "forix", "jsp2003");
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
            }*/

            FileType file = new FileType(@"C:\Test\wesh.txt");
            Console.WriteLine(file.Path);
            Console.WriteLine(file.Name);
        }
    }
}
