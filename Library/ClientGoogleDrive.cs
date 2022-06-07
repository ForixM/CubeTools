using System;
using System.Collections.Generic;
using System.IO;
using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.LibraryGoogleDrive;
using Library.ManagerReader;
using File = Google.Apis.Drive.v3.Data.File;

namespace Library
{
    public class ClientGoogleDrive : Client
    {
        //private OnedriveClient _clientOneDrive;

        public ClientGoogleDrive(ClientType type) : base(type)
        {
            //_clientOneDrive = new OnedriveClient();
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

        public override Pointer? Copy(Pointer pointer)
        {
            GoogleDriveFile file =
                new GoogleDriveFile(ManagerFile.CopyFile(((GoogleDriveFile)pointer).Id));
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

        public override void UploadFile(FileLocalPointer localPointer, Pointer destination)
        {
            ManagerFile.UploadFile(localPointer.Path, localPointer.Name, localPointer.Type, ((GoogleDriveFile)destination).Id);
        }

        public override void UploadFile(Client source, Pointer localPointer, Pointer destination)
        {
            ManagerFile.UploadFile(localPointer.Path, localPointer.Name, "application/vnd.google-apps.folder", ((GoogleDriveFile)destination).Id);
        }

        public override void UploadFolder(Client source, Pointer localPointer, Pointer destination)
        {
            throw new NotImplementedException();
        }

        public override void AccessPath(Pointer destination)
        {
            throw new NotImplementedException();
            //destination.Path = FileReader.GetPathFromFile(((GoogleDriveFile) destination).Id);
        }

        public override Pointer? GetItem(string path, bool isAbsolute = false)
        {
            string fileId = FileReader.GetFileIdFromPath(path);
            
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
            List<Google.Apis.Drive.v3.Data.File> files = FileReader.ListFileAndFolder(((GoogleDriveFile) this.CurrentFolder).Id);
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