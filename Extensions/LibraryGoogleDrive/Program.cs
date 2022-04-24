using System;

namespace LibraryGoogleDrive
{
    public class Program
    {
        private static void Main(string[] args)
        {
            GoogleDriveFile file = new GoogleDriveFile(FileReader.GetFileId("fichiertest"));
            
            Console.WriteLine(file.FileType);
        }
    }
}