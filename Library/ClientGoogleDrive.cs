using Google.Apis.Drive.v3;
using Library.LibraryGoogleDrive;
using File = Google.Apis.Drive.v3.Data.File;

namespace Library
{
    public class ClientGoogleDrive : Client
    {
        public static DriveService Service = OAuth.GetDriveService();

        public ClientGoogleDrive() : base(ClientType.GOOGLEDRIVE)
        {
            try
            {
                var service = ClientGoogleDrive.Service;
                FilesResource.GetRequest getRequest = service.Files.Get("root");
                File getFile = getRequest.Execute();
                Root = new GoogleDriveFile(getFile);
                CurrentFolder = new GoogleDriveFile(getFile);
                Children = ListChildren();
            }
            catch (Exception e)
            {
                LogErrors.LogErrors.LogWrite("exception: ", e);
            }
        }
        
        
        #region Actions

        public override Pointer CreateFile(string name)
        {
            var id = ManagerFile.CreateFile(((GoogleDriveFile) this.CurrentFolder).Id, name);
            var service = ClientGoogleDrive.Service;
            FilesResource.GetRequest getRequest = service.Files.Get(id);
            File getFile = getRequest.Execute();
            GoogleDriveFile file =
                new GoogleDriveFile(getFile);
            return file;
        }

        public override Pointer CreateFolder(string name)
        {
            var id = ManagerFile.CreateFolder(((GoogleDriveFile) this.CurrentFolder).Id, name);
            var service = ClientGoogleDrive.Service;
            FilesResource.GetRequest getRequest = service.Files.Get(id);
            File getFile = getRequest.Execute();
            GoogleDriveFile file =
                new GoogleDriveFile(getFile);
            return file;
        }

        public override Pointer? Copy(Pointer pointer, Pointer destination)
        {
            var service = ClientGoogleDrive.Service;
            string id = ManagerFile.CopyFile(((GoogleDriveFile) pointer).Id);
            FilesResource.GetRequest getRequest = service.Files.Get(id);
            File getFile = getRequest.Execute();
            GoogleDriveFile file = new GoogleDriveFile(getFile);
            string parentId = ((GoogleDriveFile) destination).Id;

            id = ManagerFile.ChangeParentsFile(file.Id, parentId);
            getRequest = service.Files.Get(id);
            getFile = getRequest.Execute();
            file = new GoogleDriveFile(getFile);
            return file;
        }

        public override void Delete(Pointer pointer)
        {
            ManagerFile.DeleteFile(((GoogleDriveFile)pointer).Id);
        }

        public override void Rename(Pointer pointer, string newName)
        {
            ManagerFile.Rename(((GoogleDriveFile)pointer).Id, newName);
        }

        public override LocalPointer DownloadFile(Client source, Pointer pointer, Pointer destination)
        {
            ManagerFile.DownloadFile(((GoogleDriveFile)pointer).Id, destination.Path);
            return base.DownloadFile(source, pointer, destination);
        }

        public override void UploadFile(Client source, Pointer localPointer, Pointer destination)
        {
            ManagerFile.UploadFile(localPointer.Path, localPointer.Name, MimeTypes.GetMimeType(localPointer.Name), ((GoogleDriveFile)destination).Id);
        }

        public override void UploadFolder(Client source, Pointer localPointer, Pointer destination)
        {
            ManagerFile.UploadFile(localPointer.Path, localPointer.Name, localPointer.Type, ((GoogleDriveFile)destination).Id);
        }

        public override void AccessPath(Pointer destination)
        {
            var service = ClientGoogleDrive.Service;
            FilesResource.GetRequest getRequest = service.Files.Get(((GoogleDriveFile)destination).Id);
            getRequest.Fields = "parents, id, name, size, mimeType";
            File getFile = getRequest.Execute();
            CurrentFolder = new GoogleDriveFile(getFile);
            foreach (var pointer in Children) pointer.Dispose();
            GC.Collect();
            Children.Clear();
            Children = ListChildren();
        }

        public override Pointer? GetItem(string path, bool isAbsolute = false)
        {
            if (!isAbsolute)
            {
                foreach (Pointer pointer in Children)
                {
                    if (pointer.Name == path) return pointer;
                }

                return null;
            }
            if (!isAbsolute) path = CurrentFolder.Path + "/" + path;
            string fileId = FileReader.GetFileIdFromPath(path);
            if (fileId is null) return null;
            
            var service = ClientGoogleDrive.Service;
            FilesResource.GetRequest getRequest = service.Files.Get(fileId);
            File getFile = getRequest.Execute();

            GoogleDriveFile file = new GoogleDriveFile(getFile);
            return file;
        }

        public override Pointer GetParentReference(Pointer pointer)
        {
            var service = ClientGoogleDrive.Service;
            // FilesResource.GetRequest getRequest = service.Files.Get(((GoogleDriveFile) pointer).Id);
            // File file = getRequest.Execute();
            // string fileId = FileReader.GetFileIdFromPath(((GoogleDriveFile)pointer).Parents[0]);
            string fileId = ((GoogleDriveFile)pointer).Parents[0];
            FilesResource.GetRequest getRequest = service.Files.Get(fileId);
            File file = getRequest.Execute();
            // string parent = FileReader.GetFileParent(((GoogleDriveFile) pointer).Id);

            GoogleDriveFile fileParent = new GoogleDriveFile(file);

            return fileParent;
        }

        public override List<Pointer>? ListChildren()
        {
            List<Google.Apis.Drive.v3.Data.File> files = FileReader.ListFileAndFolder(((GoogleDriveFile) CurrentFolder).Id);

            List<Pointer> items = new List<Pointer>();
            foreach (var i in files)
            {
                
                Pointer pointer = new GoogleDriveFile(i);
                items.Add(pointer);
            }

            return items;
        }

        #endregion
        
        #region Properties

        public override string GetItemName(Pointer pointer)
        {
            return FileReader.GetFileName(((GoogleDriveFile)pointer).Id);
        }

        public override long GetItemSize(Pointer pointer)
        {
            return FileReader.GetFileSize(((GoogleDriveFile)pointer).Id);
        }

        public override string GetItemType(Pointer pointer)
        {
            return FileReader.GetFileType(((GoogleDriveFile)pointer).Id);
        }

        public override void InitializeProperties(Pointer pointer)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}