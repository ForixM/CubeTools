using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.LibraryFtp;
using Library.ManagerExceptions;

namespace Library
{
    public class ClientTransferProtocol : Client
    {
        private ClientFtp _clientFtp;

        public string Host => _clientFtp.Host;
        public string Username => _clientFtp.Username;
        public string Password => _clientFtp.Password;
        
        public ClientTransferProtocol(string host, string username, string password) : base(ClientType.FTP)
        {
            _clientFtp = new ClientFtp(host, username, password);
            CurrentFolder = FtpFolder.ROOT;
            Root = FtpFolder.ROOT;
            foreach (var item in _clientFtp.ListDirectory((FtpFolder)CurrentFolder).Items) Children.Add(item);
        }
        
        #region Actions

        public override Pointer CreateFile(string name) => _clientFtp.CreateFile((FtpFolder) CurrentFolder!, name);

        public override Pointer CreateFolder(string name) => _clientFtp.CreateFolder((FtpFolder) CurrentFolder!, name);

        public override Pointer? Copy(Pointer pointer, Pointer destination) => null;

        public override void Delete(Pointer pointer) => _clientFtp.Delete((FtpPointer) pointer);

        public override void Rename(Pointer pointer, string newName) =>_clientFtp.Rename((FtpPointer) pointer, newName);

        public override LocalPointer DownloadFile(Client source, Pointer pointer, Pointer destination)
        {
            _clientFtp.DownloadFile((FtpFile) pointer, destination.Path);
            return base.DownloadFile(source, pointer, destination);
        }

        public override LocalPointer DownloadFolder(Client source, Pointer pointer, Pointer destination)
        {
            // TODO Implement FTP
            throw new NotImplementedException();
            return base.DownloadFolder(source, pointer, destination);
        }

        public override void UploadFile(Client source, Pointer localPointer, Pointer destination) =>  _clientFtp.UploadFile((LocalPointer) localPointer, (FtpFolder) destination);

        public override void UploadFolder(Client source, Pointer localPointer, Pointer destination) => _clientFtp.UploadFolder((LocalPointer) localPointer, (FtpFolder) destination);

        public override void AccessPath(Pointer destination)
        {
            DisposeChildren();
            foreach (var item in _clientFtp.ListDirectory((FtpFolder) destination).Items) Children.Add(item);
            CurrentFolder = destination;
        }

        public override Pointer? GetItem(string path, bool isAbsolute = false)
        {
            if (path[^1] is '/' or '\\')
                path = path.Remove(path.Length - 1);
            string name = Path.GetFileName(path);
            int last = path.Length;
            string parentPath = path.Remove(last - name.Length, name.Length);
            return _clientFtp.ListDirectory(parentPath).Items.FindLast(item => item.Name == name);
        }
        
        public override List<Pointer>? ListChildren()
        {
            if (CurrentFolder is null) return null;
            var result = new List<Pointer>();
            result.AddRange(_clientFtp.ListDirectory((FtpFolder) CurrentFolder).Items);
            return result;
        }

        #endregion
        
        #region Properties

        public override string GetItemName(Pointer pointer)
        {
            throw new NotImplementedException();
        }

        public override long GetItemSize(Pointer pointer)
        {
            throw new NotImplementedException();
        }

        public override string GetItemType(Pointer pointer)
        {
            throw new NotImplementedException();
        }

        public override void InitializeProperties(Pointer pointer)
        {
            throw new NotImplementedException();
        }

        public override Pointer GetParentReference(Pointer pointer) => ((FtpFolder) pointer).Parent;

        #endregion

    }
}