using System;
using Manager;
using MimeTypes;

namespace Onedrive
{
    internal class Program
    {
        private static OnedriveClient client;
        static void Main(string[] args)
        {
            client = new OnedriveClient();
            FileType file = new FileType("C:/Users/forix/Desktop/test.txt");
            client.authenticated += sender => Console.WriteLine(client.UploadFile(file));
            // FileType file = new FileType("C:/Users/forix/Documents/révision.xlsx");
            // Console.WriteLine(MimeTypeMap.GetMimeType(file.Path));
        }
    }
}
