using System;
using System.Collections.Generic;
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

            client.uploadFinished += (sender, success) =>
                Console.WriteLine(success ? "Upload finished" : "Error in upload");
            
            client.authenticated += delegate(object sender, bool success)
            {
                if (!success) return;
                OneArboresence arboresence = client.GetArboresence();
                Console.WriteLine(arboresence.count);
                IList<OneItem> items = arboresence.value;
                client.UploadFile(new FileType("C:/Users/forix/Documents/wow.txt"));
            };
        }
    }
}
