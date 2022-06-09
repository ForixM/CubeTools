using System.Collections.Generic;
using System.IO;

namespace Library.LibraryGoogleDrive
{
    public class GoogleDriveFile : Pointer
    {
        protected string _id;
        protected string _descrpition;
        protected string _parents;
        
        public string Id { get => _id; set => _id = value; }
        public string Description { get => _descrpition; set => _descrpition = value; } 
        public string Parents { get => _parents; set => _parents = value; }
        
        public GoogleDriveFile(string fileId)
        {
            _id = fileId;
            var Service = ClientGoogleDrive.Service;
            var request = Service.Files.Get(fileId);
            request.Fields = "size, mimeType, parents, name, description";
            var driveFile = request.Execute();
            _type = driveFile.MimeType;
            _name = driveFile.Name;
            _size = driveFile.Size ?? 0;
            _parents = (driveFile.Parents is not null) ? driveFile.Parents[0] : "root";
            _descrpition = driveFile.Description;
            _path = FileReader.GetPathFromFile(fileId);
            IsDir = driveFile.MimeType == "application/vnd.google-apps.folder";
        }
    }
}