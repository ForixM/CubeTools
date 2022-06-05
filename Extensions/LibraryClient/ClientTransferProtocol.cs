using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerExceptions;
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
            Root = FtpFolder.ROOT;
            foreach (var item in _clientFtp.ListDirectory((FtpFolder)CurrentFolder).Items) Children.Add(item);
        }
        
        #region Actions

        public override RemoteItem CreateFile(string name) => _clientFtp.CreateFile((FtpFolder) CurrentFolder!, name);

        public override RemoteItem CreateFolder(string name) => _clientFtp.CreateFolder((FtpFolder) CurrentFolder!, name);

        public override RemoteItem? Copy(RemoteItem item) => null;

        public override void Delete(RemoteItem item) => _clientFtp.Delete((IFtpItem) item);

        public override void Rename(RemoteItem item, string newName) =>_clientFtp.Rename((IFtpItem) item, newName);

        public override Pointer DownloadFile(RemoteItem item, DirectoryPointer destination)
        {
            _clientFtp.DownloadFile((FtpFile) item, destination.Path);
            return base.DownloadFile(item, destination);
        }

        public override Pointer DownloadFolder(RemoteItem item, DirectoryPointer destination)
        {
            // TODO Implement FTP
            throw new NotImplementedException();
            return base.DownloadFolder(item, destination);
        }

        public override void UploadFile(FilePointer pointer, RemoteItem destination) =>  _clientFtp.UploadFile(pointer, (FtpFolder) destination);

        public override void UploadFolder(DirectoryPointer pointer, RemoteItem destination) => _clientFtp.UploadFolder(pointer, (FtpFolder) destination);

        public override void AccessPath(RemoteItem destination)
        {
            foreach (var item in Children) item.Dispose();
            Children.Clear();
            foreach (var item in _clientFtp.ListDirectory((FtpFolder) destination).Items) Children.Add(item);
            CurrentFolder = destination;
        }

        public override RemoteItem? GetItem(string path, bool isAbsolute = false)
        {
            string name = System.IO.Path.GetFileName(path);
            int last = path.Length;
            string parentPath = path.Remove(last - name.Length, name.Length);
            return _clientFtp.ListDirectory(parentPath).Items.FindLast(item => item.Name == name);
        }
        
        public override List<RemoteItem>? ListChildren()
        {
            if (CurrentFolder is null) return null;
            var result = new List<RemoteItem>();
            result.AddRange(_clientFtp.ListDirectory((FtpFolder) CurrentFolder).Items);
            return result;
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

        public override RemoteItem GetParentReference(RemoteItem item) => ((FtpFolder) item).Parent;

        #endregion

    }
}