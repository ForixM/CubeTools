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
        }
        
        #region Configuration

        protected override void LoadConfiguration(ClientType type)
        {
        }
        
        #endregion
        
        #region Actions

        public override RemoteItem CreateFile(string name) => _clientFtp.CreateFile((FtpFolder) CurrentFolder!, name);
        public override RemoteItem CreateFolder(string name) => _clientFtp.CreateDirectory((FtpFolder) CurrentFolder!, name);

        public override RemoteItem? Copy(RemoteItem item) => null;

        public override void Delete(RemoteItem item) => _clientFtp.DeleteItem((IFtpItem) item);

        public override void Rename(RemoteItem item, string newName) => _clientFtp.Rename((IFtpItem) item, newName);

        public override Pointer Download(RemoteItem item, string name)
        {
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

        public override bool ItemExist(RemoteItem folder, RemoteItem item) => _clientFtp.ItemExist((FtpFolder) folder, (IFtpItem) item);

        public override RemoteItem? GetItem(string name) =>
            CurrentFolder is FtpFolder ? _clientFtp.GetItem((FtpFolder) CurrentFolder, name) : null;

        #endregion
    }
}