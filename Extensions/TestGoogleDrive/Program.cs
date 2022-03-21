using System;

namespace TestGoogleDrive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("hello");
            LibraryGoogleDrive.FileReader.ListFileAndFolder("test");
        }
    }
}