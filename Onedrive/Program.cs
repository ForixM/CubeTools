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
                    if (oneItem.name == "Document.docx" && oneItem.Type == OneItemType.FILE)
                    {
                        file = oneItem;
                    }

                    if (oneItem.name == "F1" && oneItem.Type == OneItemType.FOLDER)
                    {
                        f1 = oneItem;
                    }

                    if (f1 != null && file != null) break;
                }

                Console.WriteLine(client.Copy(file, f1));
            };
        }
    }
}
