using System.Collections.Generic;
using System.IO;
using File = Google.Apis.Drive.v3.Data.File;

namespace Library.LibraryGoogleDrive
{
    public class GoogleDriveFile : Pointer
    {
        protected string _id;
        protected string _descrpition;
        protected IList<string> _parents;
        
        public string Id { get => _id; set => _id = value; }
        public string Description { get => _descrpition; set => _descrpition = value; } 
        public IList<string> Parents { get => _parents; set => _parents = value; }
        
        // public GoogleDriveFile(string fileId)
        // {
        //     _id = fileId;
        //     var Service = ClientGoogleDrive.Service;
        //     var request = Service.Files.Get(fileId);
        //     request.Fields = "size, mimeType, parents, name, description";
        //     var driveFile = request.Execute();
        //     _type = driveFile.MimeType;
        //     _name = driveFile.Name;
        //     _size = driveFile.Size ?? 0;
        //     _parents = (driveFile.Parents is not null) ? driveFile.Parents[0] : "root";
        //     _descrpition = driveFile.Description;
        //     Thread test = new Thread(() =>
        //     {
        //         _path = FileReader.GetPathFromFile(fileId);
        //     });
        //     test.Start();
        //     IsDir = driveFile.MimeType == "application/vnd.google-apps.folder";
        // }

        public GoogleDriveFile(File file)
        {
            _id = file.Id;
            // var Service = ClientGoogleDrive.Service;
            // var request = Service.Files.Get(fileId);
            // request.Fields = "size, mimeType, parents, name, description";
            // var driveFile = request.Execute();
            _type = ManagerReader.ManagerReader.GetFileExtension(file.Name);
            // _type = file.MimeType;
            _name = file.Name;
            _size = file.Size ?? 0;
            _parents = file.Parents ?? new List<string>{"root"};
            _descrpition = file.Description;
            _path = PathFromFile(file);
            IsDir = file.MimeType == "application/vnd.google-apps.folder";
        }

        private string PathFromFile(File file)
        {
            // var Service = ClientGoogleDrive.Service;
            string pathRes = file.Name;
            
            // var request = Service.Files.Get(fileId);
            // request.Fields = "parents, name";
            // var driveFile = request.Execute();
            
            if (file.Parents != null)
            {
                foreach (string parent in file.Parents)
                {
                    pathRes = parent + "/" + pathRes;
                }
            }

            return pathRes;

            // while (true)
            // {
            //     if (file.Parents == null)
            //     {
            //         return pathRes;
            //     }
            //     pathRes = '/' + driveFile.Name + pathRes;
            //     fileId = driveFile.Parents[0];
            //     request = Service.Files.Get(fileId);
            //     request.Fields = "parents, name";
            //     driveFile = request.Execute();
            // }
        }
    }
}