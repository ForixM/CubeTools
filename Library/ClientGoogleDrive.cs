using Google.Apis.Drive.v3;
using Library.LibraryGoogleDrive;

namespace Library
{
    public class ClientGoogleDrive : Client
    {
        public static DriveService Service;

        static ClientGoogleDrive()
        {
            Service = OAuth.GetDriveService();
        }

        public ClientGoogleDrive() : base(ClientType.GOOGLEDRIVE)
        {
            Root = new GoogleDriveFile("root");
            CurrentFolder = new GoogleDriveFile("root");
            Children = ListChildren();
        }
        
        
        #region Actions

        public override Pointer CreateFile(string name)
        {
            GoogleDriveFile file =
                new GoogleDriveFile(ManagerFile.CreateFile(((GoogleDriveFile) this.CurrentFolder).Id, name));
            return file;
        }

        public override Pointer CreateFolder(string name)
        {
            GoogleDriveFile file =
                new GoogleDriveFile(ManagerFile.CreateFolder(((GoogleDriveFile) this.CurrentFolder).Id, name));
            return file;
        }

        public override Pointer? Copy(Pointer pointer, Pointer destination)
        {
            GoogleDriveFile file = new GoogleDriveFile(ManagerFile.CopyFile(((GoogleDriveFile)pointer).Id));
            string parentId = ((GoogleDriveFile) destination).Id;
            file = new GoogleDriveFile(ManagerFile.ChangeParentsFile(file.Id, parentId));
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

        public override void UploadFile(Client source, Pointer localPointer, Pointer destination)
        {
            ManagerFile.UploadFile(localPointer.Path, localPointer.Name, "application/vnd.google-apps.folder", ((GoogleDriveFile)destination).Id);
        }

        public override void UploadFolder(Client source, Pointer localPointer, Pointer destination)
        {
            ManagerFile.UploadFile(localPointer.Path, localPointer.Name, localPointer.Type, ((GoogleDriveFile)destination).Id);
        }

        public override void AccessPath(Pointer destination)
        {
            CurrentFolder = new GoogleDriveFile(FileReader.GetFileIdFromPath(destination.Path));
            foreach (var pointer in Children) pointer.Dispose();
            GC.Collect();
            Children.Clear();
            Children = ListChildren();
        }

        public override Pointer? GetItem(string path, bool isAbsolute = false)
        {
            if (!isAbsolute) path = CurrentFolder.Path + "/" + path;
            string fileId = FileReader.GetFileIdFromPath(path);
            if (fileId is null) return null;
            
            GoogleDriveFile file = new GoogleDriveFile(fileId);
            return file;
        }

        public override Pointer GetParentReference(Pointer pointer)
        {
            string parent = FileReader.GetFileParent(((GoogleDriveFile) pointer).Id);

            GoogleDriveFile fileParent = new GoogleDriveFile(parent);

            return fileParent;
        }

        public override List<Pointer>? ListChildren()
        {
            List<Google.Apis.Drive.v3.Data.File> files = FileReader.ListFileAndFolder(((GoogleDriveFile) CurrentFolder).Id);
            List<Pointer> items = new List<Pointer>();
            foreach (var i in files)
            {
                Pointer pointer = new GoogleDriveFile(i.Id);
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