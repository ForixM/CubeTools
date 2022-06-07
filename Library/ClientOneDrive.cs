using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.LibraryOneDrive;
using Library.LibraryFtp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library
{
    public class ClientOneDrive : Client
    {
        private OnedriveClient _clientOneDrive;
        public OnedriveClient Client => _clientOneDrive;

        public ClientOneDrive() : base(ClientType.ONEDRIVE)
        {
            _clientOneDrive = new OnedriveClient();
            Root = OnePointer.ROOT;
            CurrentFolder = OnePointer.ROOT;
            ((OnePointer)CurrentFolder).SetVariables();
        }
        
        
        #region Actions

        public override Pointer CreateFile(string name)
        {
            Task<bool> createTask = _clientOneDrive.CreateFile(name, (OnePointer)CurrentFolder);
            if (createTask.Result)
            {
                return GetItem(name, true);
            }

            return null;
        }

        public override Pointer CreateFolder(string name) => _clientOneDrive.CreateFolder(name, (OnePointer)CurrentFolder);

        public override Pointer? Copy(Pointer pointer) //TODO
        {
            throw new NotImplementedException();
        }

        public override void Delete(Pointer pointer)
        {
            _clientOneDrive.DeleteItem((OnePointer)pointer);
        }

        public override void Rename(Pointer pointer, string newName)
        {
            if (!_clientOneDrive.RenameItem((OnePointer) pointer, newName))
            {
                //TODO: Error popup, can't rename this file
            }
        }

        public override LocalPointer DownloadFile(Client source, Pointer pointer, Pointer destination)
        {
            _clientOneDrive.DownloadFile(destination.Path, (OnePointer)pointer);
            return base.DownloadFile(source, pointer, destination);
        }

        public override LocalPointer DownloadFolder(Client source, Pointer pointer, Pointer destination)
        {
            _clientOneDrive.DownloadFolder(destination.Path, (OnePointer)pointer);
            return base.DownloadFolder(source, pointer, destination);
        }

        public override void UploadFile(Client source, Pointer localPointer, Pointer destination)
        {
            _clientOneDrive.UploadFile((LocalPointer) localPointer, (OnePointer) destination);
        }

        public override void UploadFolder(Client source, Pointer localPointer, Pointer destination)
        {
            _clientOneDrive.UploadFolder((LocalPointer) localPointer, (OnePointer) destination);
        }

        public override void AccessPath(Pointer destination)
        {
            DisposeChildren();
            CurrentFolder = destination;
            foreach (OnePointer item in _clientOneDrive.GetArboresence((OnePointer)destination).items) Children.Add(item);
        }

        public override Pointer? GetItem(string path, bool isAbsolute = false)
        {
            if (isAbsolute)
            {
                try
                {
                    OnePointer pointer =
                        JsonConvert.DeserializeObject<OnePointer>(_clientOneDrive.GetItemFullMetadata(path).ToString());
                    pointer.SetVariables();
                    return pointer;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            return _clientOneDrive.GetArboresence((OnePointer) CurrentFolder!).items
                .FirstOrDefault(oneObject => oneObject.name == path);
        }
        
        public override Pointer GetParentReference(Pointer pointer)
        {
            JObject jObject = _clientOneDrive.GetItemFullMetadata(pointer.ParentPath);
            OnePointer parentPointer =  JsonConvert.DeserializeObject<OnePointer>(jObject.ToString());
            if (parentPointer.isRoot) parentPointer = OnePointer.ROOT;
            parentPointer.SetVariables();
            return parentPointer;
        }

        public override List<Pointer>? ListChildren()
        {
            List<Pointer> result = new List<Pointer>();
            result.AddRange(_clientOneDrive.GetArboresence((OnePointer) CurrentFolder!).items);
            return result;
        }

        #endregion
        
        #region Properties

        public override string GetItemName(Pointer pointer)
        {
            return pointer.Name;
        }

        public override long GetItemSize(Pointer pointer)
        {
            return pointer.Size;
        }

        public override string GetItemType(Pointer pointer)
        {
            return pointer.Type;
        }

        public override void InitializeProperties(Pointer pointer)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Others

        #endregion
    }
}