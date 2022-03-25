using System;

namespace TestGoogleDrive
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("hello");
            var fileID = LibraryGoogleDrive.FileReader.GetFileId("fileTest");
            LibraryGoogleDrive.GoogleDriveFile.DeleteFile(fileID);
        }
    }
}