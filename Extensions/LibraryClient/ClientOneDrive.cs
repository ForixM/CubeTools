using Library;
using LibraryClient.LibraryOneDrive;

namespace LibraryClient
{
    public class ClientOneDrive : Client
    {
        private OnedriveClient _clientOneDrive;

        public ClientOneDrive(ClientType type) : base(type)
        {
            _clientOneDrive = new OnedriveClient();
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

        public override RemoteItem? GetItem(string path)
        {
            throw new NotImplementedException();
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

        public override RemoteItem GetParentReference(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}