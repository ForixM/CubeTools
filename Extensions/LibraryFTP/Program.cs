using System;
using System.Threading.Tasks;
using FluentFTP;
using Library.Pointers;
using SevenZip;

namespace LibraryFTP
{
    internal class Program
    {
        private static double _progress;

        private static void Main(string[] args)
        {
            ClientFtp ftp = new ClientFtp("127.0.0.1","forix", "lolmdr");
            FtpArboresence arbo = ftp.ListRoot();
            Console.WriteLine(arbo.ToString());
            // ftp.MakeDirectory(FtpFolder.ROOT, "test");
            // ftp.progress += (sender, transfered, bytes) =>
            //     Console.WriteLine($"Progress={transfered * 100 / bytes}%");
            // ftp.finish += (sender, success) =>
            //     Console.WriteLine(success ? "Download finished" : "Error in download");
            // ftp.UploadFile(new FileType("C:/Users/forix/Desktop/machintosh.jpg"), FtpFolder.ROOT);
            foreach (IFtpItem arboItem in arbo.Items)
            {
                if (arboItem.Name == "machintosh.jpg")
                {
                    // ftp.DeleteItem(arboItem);
                    // ftp.progress += (sender, transfered, bytes) =>
                    //     Console.WriteLine($"Progress={transfered * 100 / bytes}%");
                    // ftp.finish += (sender, success) =>
                    //     Console.WriteLine(success ? "Download finished" : "Error in download");
                    // ftp.DownloadFile((FtpFile) arboItem, "C:/Users/forix/Desktop/machintosh.jpg");
                }
                if (arboItem is FtpFolder item)
                {
                    Console.WriteLine();
                    FtpArboresence arbo2 = ftp.ListDirectory(item);
                    Console.WriteLine(arbo2.ToString());
                    if (item.Name == "test")
                    {
                        ftp.DeleteItem(item);
                    }
                }
            }

        }
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

        private static void PrintProgress(double percent)
        {
            if (percent - _progress >= 5)
            {
                _progress = percent;
                Console.Clear();
                Console.Write("|");
                var i = 0;
                for (; i < percent * 20 / 100; i++) Console.Write("#");

                for (; i < 20; i++) Console.Write(".");

                Console.WriteLine("| " + percent + "%");
            }
        }
    }
}