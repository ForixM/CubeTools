using System;
using FluentFTP;
using SevenZip;

namespace LibraryFTP
{
    internal class Program
    {
        private static double _progress;

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

        private static void Main(string[] args)
        {
        }
    }
}