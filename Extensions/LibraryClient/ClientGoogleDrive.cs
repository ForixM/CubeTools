﻿using Library;
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
            throw new NotImplementedException();
        }

        public override RemoteItem CreateFolder(string name)
        {
            throw new NotImplementedException();
        }

        public override RemoteItem? Copy(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        public override void Delete(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        public override void Rename(RemoteItem item, string newName)
        {
            throw new NotImplementedException();
        }

        public override Pointer Download(RemoteItem item, string name)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public override long GetItemSize(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        public override string GetItemType(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        public override void InitializeProperties(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Others

        public override RemoteItem GetRoot()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}