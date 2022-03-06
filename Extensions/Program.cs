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
            PrintProgress(progress.PercentDone);
        }

        private static void CompressionDone(object sender, EventArgs args)
        {
            Console.WriteLine("Compression done");
        }

        private static void FtpProgress(object sender, FtpProgress progress)
        {
            // PrintProgress(progress.Progress);
        }

        private static double _progress = 0;

        private static void PrintProgress(double percent)
        {
            if (percent - _progress >= 5)
            {
                _progress = percent;
                Console.Clear();
                Console.Write("|");
                int i = 0;
                for (; i < percent * 20 / 100; i++)
                {
                    Console.Write("#");
                }

                for (; i < 20; i++)
                {
                    Console.Write(".");
                }

                Console.WriteLine("| "+percent+"%");
            }
        }
        
        static void Main(string[] args)
        {
            string ans;
            FtpUtils ftp = null;
            do
            {
                Console.WriteLine("Please type a command:");
                ans = Console.ReadLine();
                switch (ans)
                {
                    case "compress init":
                        Compression.Init();
                        break;
                    case "compress folder":
                        Task task = Compression.CompressDirectory(new DirectoryType("C:/Compression Test/a"), OutArchiveFormat.SevenZip, Progress, CompressionDone);
                        task.Wait();
                        break;
                    case "ftp init":
                        ftp = new FtpUtils("127.0.0.1", "forix", "lolmdr", false);
                        break;
                    case "ftp upload":
                        Task<FtpStatus> status = ftp.UploadFile("/file.txt", new FileType("C:/Compression Test/a/file.txt"));
                        status.Wait();
                        break;
                    case "ftp download":
                        Task<FtpStatus> status2 = ftp.DownloadFile("/file.txt", "C:/Compression Test/file.txt");
                        status2.Wait();
                        break;
                    case "quit":
                        Console.WriteLine("Goodbye !");
                        break;
                    default:
                        Console.WriteLine("Unknown command. Please retry");
                        break;
                }
            } while (ans != "quit");
            // FileType[] files = new FileType[2];
            // files[0] = new FileType("C:/Test/MinecraftInstaller.exe");
            // files[1] = new FileType("C:/Test/7z.dll");
            // Task task = Compression.CompressFiles(files, "C:/Test/compressed.7z", OutArchiveFormat.SevenZip, Progress);
            // Task task = Compression.CompressDirectory(new DirectoryType("C:/Compression Test/a"), OutArchiveFormat.SevenZip, Progress);
            // long milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            // while (!task.IsCompleted)
            // {
            //     
            // }
            // long milliseconds2 = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            // Console.WriteLine("time: "+(milliseconds2-milliseconds)+"ms");


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