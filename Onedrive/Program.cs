using System;
using System.Collections.Generic;
using Manager;
using Manager.Pointers;
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
                IList<OneItem> items = arboresence.items;
                
                
                OneItem f1 = null;
                OneItem file = null;
                foreach (OneItem oneItem in items)
                {
                    // if (oneItem.name == "wow.txt" && oneItem.Type == OneItemType.FILE)
                    // {
                    //     file = oneItem;
                    // }

                    if (oneItem.name == "F1" && oneItem.Type == OneItemType.FOLDER)
                    {
                        f1 = oneItem;
                        break;
                    }

                    // if (f1 != null && file != null) break;
                }

                client.uploadUpdate += delegate(object o, int percent)
                {
                    Console.WriteLine($"Progression={percent}%");
                };
                client.uploadFinished += (o, b) => Console.WriteLine(b ? "Upload finished." : "Error in upload");
                Console.WriteLine(client.UploadFile(new FileType("C:/Users/forix/Desktop/wow.txt"), f1));
                // Console.WriteLine(client.CreateShareLink(file, SharePermission.READONLY));
                // client.DownloadFile("C:/Users/forix/Desktop/wow.txt", file);
                // Console.WriteLine(client.Copy(file, f1));
            };
        }
    }
}
