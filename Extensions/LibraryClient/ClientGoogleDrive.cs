using System;
using System.Collections.Generic;
using System.IO;
using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerReader;
using LibraryClient.LibraryGoogleDrive;
using File = Google.Apis.Drive.v3.Data.File;

namespace LibraryClient
{
    public class ClientGoogleDrive : Client
    {
        //private OnedriveClient _clientOneDrive;

        public ClientGoogleDrive(ClientType type) : base(type)
        {
            //_clientOneDrive = new OnedriveClient();
        }
        
        
        #region Actions

        public override RemoteItem CreateFile(string name)
        {
            GoogleDriveFile file =
                new GoogleDriveFile(ManagerFile.CreateFile(((GoogleDriveFile) this.CurrentFolder).Id, name));
            return file;
        }

        public override RemoteItem CreateFolder(string name)
        {
            GoogleDriveFile file =
                new GoogleDriveFile(ManagerFile.CreateFolder(((GoogleDriveFile) this.CurrentFolder).Id, name));
            return file;
        }

        public override RemoteItem? Copy(RemoteItem item)
        {
            GoogleDriveFile file =
                new GoogleDriveFile(ManagerFile.CopyFile(((GoogleDriveFile)item).Id));
            return file;
        }

        public override void Delete(RemoteItem item)
        {
            ManagerFile.DeleteFile(((GoogleDriveFile)item).Id);
        }

        public override void Rename(RemoteItem item, string newName)
        {
            ManagerFile.Rename(((GoogleDriveFile)item).Id, newName);
        }

        public override Pointer DownloadFolder(RemoteItem item, DirectoryPointer destination)
        {
            throw new NotImplementedException();
        }
        
        public override Pointer DownloadFile(RemoteItem item, DirectoryPointer destination)
        {
            throw new NotImplementedException();
        }

        public override void UploadFile(FilePointer pointer, RemoteItem destination)
        {
            ManagerFile.UploadFile(pointer.Path, pointer.Name, pointer.Type, ((GoogleDriveFile)destination).Id);
        }
        
        public override void UploadFolder(DirectoryPointer pointer, RemoteItem destination)
        {
            ManagerFile.UploadFile(pointer.Path, pointer.Name, "application/vnd.google-apps.folder", ((GoogleDriveFile)destination).Id);
        }

        public override void AccessPath(RemoteItem destination)
        {
            throw new NotImplementedException();
            //destination.Path = FileReader.GetPathFromFile(((GoogleDriveFile) destination).Id);
        }

        public override RemoteItem? GetItem(string path, bool isAbsolute = false)
        {
            string fileId = FileReader.GetFileIdFromPath(path);
            
            GoogleDriveFile file = new GoogleDriveFile(fileId);
            return file;
        }

        public override RemoteItem GetParentReference(RemoteItem item)
        {
            string parent = FileReader.GetFileParent(((GoogleDriveFile) item).Id);

            GoogleDriveFile fileParent = new GoogleDriveFile(parent);

            return fileParent;
        }

        public override List<RemoteItem>? ListChildren()
        {
            List<Google.Apis.Drive.v3.Data.File> files = FileReader.ListFileAndFolder(((GoogleDriveFile) this.CurrentFolder).Id);
            List<RemoteItem> items = new List<RemoteItem>();
            foreach (var i in files)
            {
                RemoteItem item = new GoogleDriveFile(i.Id);
                items.Add(item);
            }

            return items;
        }

        #endregion
        
        #region Properties

        public override string GetItemName(RemoteItem item)
        {
            return FileReader.GetFileName(((GoogleDriveFile)item).Id);
        }

        public override long GetItemSize(RemoteItem item)
        {
            return FileReader.GetFileSize(((GoogleDriveFile)item).Id);
        }

        public override string GetItemType(RemoteItem item)
        {
            return FileReader.GetFileType(((GoogleDriveFile)item).Id);
        }

        public override void InitializeProperties(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}