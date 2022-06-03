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
        
        #region Configuration

        protected override void LoadConfiguration(ClientType type)
        {
        }
        
        #endregion
        
        #region Actions

        public override RemoteItem CreateFile(string name) => null; //_clientOneDrive.CreateFile((FtpFolder) CurrentFolder!, name);
        public override RemoteItem CreateFolder(string name) => null; //_clientOneDrive.CreateFolder( CurrentFolder!, name);

        public override RemoteItem? Copy(RemoteItem item) => null;

        public override void Delete(RemoteItem item)
        {
            return;}//_clientOneDrive.DeleteItem((OneItem) item);

        public override void Rename(RemoteItem item, string newName)
        {
            return;} //_clientOneDrive.Rename((IFtpItem) item, newName);

        public override Pointer Download(RemoteItem item, string name)
        {
            /*_clientOneDrive.DownloadFile((OneItem) item, name);
            return base.Download(item, name);*/
            return Pointer.NullPointer;
        }

        public override void Upload(Pointer pointer, RemoteItem destination)
        {
            /*
            if (pointer.IsDir) _clientFtp.UploadFolder(pointer, (FtpFolder) destination);
            else _clientFtp.UploadFile(pointer, (FtpFolder)destination);
            */
        }

        public override void AccessPath(RemoteItem destination)
        {
            /*
            Children.Clear();
            foreach (var item in _clientFtp.ListDirectory((FtpFolder) destination).Items)
                Children.Add(item);
                */
        }

        public override void Refresh()
        {
            /*
            Children.Clear();
            if (CurrentFolder is null) return;
            foreach (var item in _clientFtp.ListDirectory( (FtpFolder)CurrentFolder).Items) Children.Add(item);
            */
        }

        public override bool ItemExist(RemoteItem folder, RemoteItem item) => false; /*_clientFtp.ItemExist((FtpFolder) folder, (IFtpItem) item);*/

        public override RemoteItem? GetItem(string name) => null;
            //CurrentFolder is FtpFolder ? _clientFtp.GetItem((FtpFolder) CurrentFolder, name) : null;

        #endregion
    }
}