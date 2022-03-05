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

        private static void Progress(object sender, ProgressEventArgs progress)
        {
            Console.WriteLine(progress.PercentDone);
        }
        
        static void Main(string[] args)
        {
            Compression.Init();
            FileType[] files = new FileType[2];
            files[0] = new FileType("C:/Test/MinecraftInstaller.exe");
            files[1] = new FileType("C:/Test/7z.dll");
            // Task task = Compression.CompressFiles(files, "C:/Test/compressed.7z", OutArchiveFormat.SevenZip, Progress);
            Task task = Compression.CompressFiles(files, OutArchiveFormat.SevenZip, Progress);
            long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            while (!task.IsCompleted)
            {
                
            }
            long milliseconds2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Console.WriteLine("time: "+(milliseconds2-milliseconds)+"ms");
            
            
            // FtpUtils ftp = new FtpUtils("192.168.0.15", "forix", "abcd0000", false);
            // Console.WriteLine(ftp.Client.IsEncrypted);
            // ftp.Rename("/coucou.txt", "alors.txt");
            // List<string> files = ftp.GetFiles();
            // foreach (string file in files)
            // {
            //     Console.WriteLine(file);
            // }
            /*Console.WriteLine();
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