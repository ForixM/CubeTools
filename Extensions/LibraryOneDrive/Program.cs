using System;
using Library.Pointers;
using Onedrive;

namespace LibraryOneDrive
{
    internal class Program
    {
        private static OnedriveClient client;

        private static void Main(string[] args)
        {
            client = new OnedriveClient();
            client.authenticated += delegate(object sender, bool success)
            {
                if (!success)
                {
                    Console.Error.WriteLine("Couldn't connect");
                    return;
                }

                var arbo = client.GetArboresence();
                OneItem folder = null;
                foreach (OneItem item in arbo.items)
                {
                    if (item.name == "F1" && item.Type == OneItemType.FOLDER)
                    {
                        folder = item;
                    }
                }

                if (folder == null)
                {
                    Console.Error.WriteLine("Couldn't find folder");
                    return;
                }
                client.Rename(folder, "Test");
            };
            
            /*var file = new FileType("C:/Users/forix/Desktop/test.txt");

            client.uploadFinished += (sender, success) =>
                Console.WriteLine(success ? "Upload finished" : "Error in upload");

            client.authenticated += delegate(object sender, bool success)
            {
                if (!success) return;
                var arboresence = client.GetArboresence();
                Console.WriteLine(arboresence.count);
                var items = arboresence.items;
                OneItem f1 = null;
                OneItem file = null;
                foreach (var oneItem in items)
                {
                    if (oneItem.name == "Document.docx" && oneItem.Type == OneItemType.FILE) file = oneItem;

                    if (oneItem.name == "F1" && oneItem.Type == OneItemType.FOLDER) f1 = oneItem;

                    if (f1 != null && file != null) break;
                }

                Console.WriteLine(client.UploadFile(new FileType("C:/Users/forix/Desktop/wow.txt"), f1));
                // Console.WriteLine(client.Copy(file, f1));
            };*/
        }
    }
}