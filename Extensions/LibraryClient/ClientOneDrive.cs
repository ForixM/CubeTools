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

        public override RemoteItem CreateFile(string name) //TODO
        {
            throw new NotImplementedException();
        }

        public override RemoteItem CreateFolder(string name)
        {
            return _clientOneDrive.CreateFolder(name, (OneItem)CurrentFolder);
        }

        public override RemoteItem? Copy(RemoteItem item) //TODO
        {
            throw new NotImplementedException();
        }

        public override void Delete(RemoteItem item)
        {
            _clientOneDrive.DeleteItem((OneItem)item);
        }

        public override void Rename(RemoteItem item, string newName) //TODO
        {
            throw new NotImplementedException(); 
        }

        public override Pointer Download(RemoteItem item, string name)
        {
            _clientOneDrive.DownloadFile(name, (OneItem)item);
            return base.Download(item, name);
        }

        public override void Upload(Pointer pointer, RemoteItem destination)
        {
            _clientOneDrive.UploadFile(pointer, (OneItem) destination);
        }

        public override void AccessPath(RemoteItem destination)
        {
            CurrentFolder = destination;
            foreach (OneItem item in _clientOneDrive.GetArboresence((OneItem)destination).items)
            {
                Children.Add(item);
            }
        }

        public override void Refresh()
        {
            foreach (OneItem item in _clientOneDrive.GetArboresence((OneItem)CurrentFolder).items)
            {
                Children.Add(item);
            }
        }

        public override RemoteItem? GetItem(string name) //TODO
        {
            // => CurrentFolder.IsDir ? _clientOneDrive.GetItem(folder, name) : null;
            throw new NotImplementedException();
        }

        public override RemoteItem? GetItem(RemoteItem folder, string name) //TODO
        {
            throw new NotImplementedException();
        }

        public override RemoteItem GetParentReference(RemoteItem item)
        {
            throw new NotImplementedException();
        }

        public override List<RemoteItem>? ListChildren(RemoteItem folder) //TODO
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region Properties

        public override string GetItemName(RemoteItem item) //TODO
        {
            throw new NotImplementedException();
        }

        public override long GetItemSize(RemoteItem item) //TODO
        {
            throw new NotImplementedException();
        }

        public override string GetItemType(RemoteItem item) //TODO
        {
            throw new NotImplementedException();
        }

        public override void InitializeProperties(RemoteItem item) //TODO
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Others

        #endregion
    }
}