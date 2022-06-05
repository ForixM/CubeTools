using System;
using System.Collections.Generic;
using Library;
using Library.ManagerReader;
using LibraryClient.LibraryGoogleDrive;

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

        public override Pointer Download(RemoteItem item, string name)
        {
            throw new NotImplementedException();
            return base.Download(item, name);
        }

        public override void Upload(Pointer pointer, RemoteItem destination)
        {
            throw new NotImplementedException();
        }

        public override void AccessPath(RemoteItem destination)
        {
            throw new NotImplementedException();
        }

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public override RemoteItem? GetItem(string name)
        {
            string id = FileReader.GetFileId(name);
            
            if (id == null)
            {
                id = FileReader.GetFolderId(name);
            }

            GoogleDriveFile file = new GoogleDriveFile(id);
            return file;
        }

        public override RemoteItem? GetItem(RemoteItem folder, string name)
        {
            string id = FileReader.GetFileId(name, ((GoogleDriveFile) folder).Id);
            
            if (id == null)
            {
                id = FileReader.GetFolderId(name, ((GoogleDriveFile) folder).Id);
            }

            GoogleDriveFile file = new GoogleDriveFile(id);
            return file;
        }

        public override RemoteItem? GetItem(RemoteItem folder, string name)
        {
            throw new NotImplementedException();
        }

        public override List<RemoteItem>? ListChildren(RemoteItem folder)
        {
            throw new NotImplementedException();
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