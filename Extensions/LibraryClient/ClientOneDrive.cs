using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using LibraryClient.LibraryFtp;
using LibraryClient.LibraryOneDrive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibraryClient
{
    public class ClientOneDrive : Client
    {
        private OnedriveClient _clientOneDrive;
        public OnedriveClient Client => _clientOneDrive;

        public ClientOneDrive(ClientType type) : base(type)
        {
            _clientOneDrive = new OnedriveClient();
            Root = OneItem.ROOT;
            CurrentFolder = OneItem.ROOT;
            ((OneItem)CurrentFolder).SetVariables();
        }
        
        
        #region Actions

        public override RemoteItem CreateFile(string name) //TODO
        {
            throw new NotImplementedException();
        }

        public override RemoteItem CreateFolder(string name) => _clientOneDrive.CreateFolder(name, (OneItem)CurrentFolder);

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

        public override Pointer DownloadFile(RemoteItem item, DirectoryPointer destination)
        {
            _clientOneDrive.DownloadFile(destination.Path, (OneItem)item);
            return base.DownloadFile(item, destination);
        }

        public override void UploadFile(FilePointer pointer, RemoteItem destination) =>  _clientOneDrive.UploadFile(pointer, (OneItem) destination);

        public override void UploadFolder(DirectoryPointer pointer, RemoteItem destination)
        {
            // TODO Upload function
            throw new NotImplementedException();
            //_clientOneDrive.UploadFile(pointer, (OneItem) destination);
        }

        public override void AccessPath(RemoteItem destination)
        {
            DisposeChildren();
            CurrentFolder = destination;
            foreach (OneItem item in _clientOneDrive.GetArboresence((OneItem)destination).items) Children.Add(item);
        }

        public override RemoteItem? GetItem(string name, bool isAbsolute = false)
        {
            if (isAbsolute)
            {
                try
                {
                    OneItem item =
                        JsonConvert.DeserializeObject<OneItem>(_clientOneDrive.GetItemFullMetadata(name).ToString());
                    item.SetVariables();
                    return item;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return _clientOneDrive.GetArboresence((OneItem) CurrentFolder!).items
                .FirstOrDefault(oneObject => oneObject.name == name);
        }
        
        public override RemoteItem GetParentReference(RemoteItem item)
        {
            JObject jObject = _clientOneDrive.GetItemFullMetadata(item.ParentPath);
            OneItem parentItem =  JsonConvert.DeserializeObject<OneItem>(jObject.ToString());
            if (parentItem.isRoot) parentItem = OneItem.ROOT;
            parentItem.SetVariables();
            return parentItem;
        }

        public override List<RemoteItem>? ListChildren()
        {
            List<RemoteItem> result = new List<RemoteItem>();
            result.AddRange(_clientOneDrive.GetArboresence((OneItem) CurrentFolder!).items);
            return result;
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