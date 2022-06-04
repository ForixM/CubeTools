using Library;
using LibraryClient.LibraryFtp;

namespace LibraryClient
{
    public class ClientTransferProtocol : Client
    {
        private ClientFtp _clientFtp;

        public string Host => _clientFtp.Host;
        public string Username => _clientFtp.Username;
        public string Password => _clientFtp.Password;
        
        public ClientTransferProtocol(ClientType type, string host, string username, string password) : base(type)
        {
            _clientFtp = new ClientFtp(host, username, password);
            CurrentFolder = FtpFolder.ROOT;
            foreach (var item in _clientFtp.ListDirectory( (FtpFolder)CurrentFolder).Items) Children.Add(item);
        }
        
        #region Actions

        public override RemoteItem CreateFile(string name) => _clientFtp.CreateFile((FtpFolder) CurrentFolder!, name);

        public override RemoteItem CreateFolder(string name) => _clientFtp.CreateFolder((FtpFolder) CurrentFolder!, name);

        public override RemoteItem? Copy(RemoteItem item) => null;

        public override void Delete(RemoteItem item) => _clientFtp.Delete((IFtpItem) item);

        public override void Rename(RemoteItem item, string newName) =>_clientFtp.Rename((IFtpItem) item, newName);

        public override Pointer Download(RemoteItem item, string name)
        {
            // TODO CHECK
            _clientFtp.DownloadFile((FtpFile) item, name);
            return base.Download(item, name);
        }

        public override void Upload(Pointer pointer, RemoteItem destination)
        {
            if (pointer.IsDir) _clientFtp.UploadFolder(pointer, (FtpFolder) destination);
            else _clientFtp.UploadFile(pointer, (FtpFolder)destination);
        }

        public override void AccessPath(RemoteItem destination)
        {
            Children.Clear();
            foreach (var item in _clientFtp.ListDirectory((FtpFolder) destination).Items)
                Children.Add(item);
        }

        public override void Refresh()
        {
            Children.Clear();
            if (CurrentFolder is null) return;
            foreach (var item in _clientFtp.ListDirectory( (FtpFolder)CurrentFolder).Items) Children.Add(item);
        }

        public override RemoteItem? GetItem(string name) => CurrentFolder is FtpFolder folder ? _clientFtp.GetItem(folder, name) : null;
        

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