using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CubeTools;
using FluentFTP;
using Manager;
using SevenZip;

namespace Extensions
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            FtpUtils ftp = new FtpUtils("127.0.0.1", "forix", "lolmdr");

            
            /*List<string> files = ftp.GetFiles();
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


        }
    }
}
