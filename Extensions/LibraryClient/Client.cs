using Library;
using Library.DirectoryPointer;
using Library.FilePointer;

namespace LibraryClient
{
    public abstract class Client
    {

        public string Path;
        public RemoteItem? CurrentFolder;
        public ClientType Type;
        public List<RemoteItem> Children;
        
        public Client(ClientType type)
        {
            // Attributes
            Type = type;
            Children = new List<RemoteItem>();
            Path = "";
            LoadConfiguration(type);
            CurrentFolder = null;
        }
        
        #region Configuration
        protected abstract void LoadConfiguration(ClientType type);
        
        #endregion
        
        #region Actions

        /// <summary>
        /// Create a file in the current folder
        /// </summary>
        /// <param name="name">the name of this file with its extension</param>
        /// <returns>The remote item created</returns>
        public abstract RemoteItem CreateFile(string name);
        
        /// <summary>
        /// Create a folder in the current folder
        /// </summary>
        /// <param name="name">the name of the folder</param>
        /// <returns>the created folder</returns>
        public abstract RemoteItem CreateFolder(string name);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract RemoteItem? Copy(RemoteItem item);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public abstract void Delete(RemoteItem item);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newName"></param>
        public abstract void Rename(RemoteItem item, string newName);

        /// <summary>
        /// Download the file or folder given by its name (name)
        /// </summary>
        /// <param name="item">the folder of reference</param>
        /// <param name="name">the name of the file / folder to download</param>
        /// <returns></returns>
        public virtual Pointer Download(RemoteItem item, string name)
        {
            if (System.IO.File.Exists(name)) return new FilePointer(name);
            else return new DirectoryPointer(name);
        }
        public abstract void Upload(Pointer pointer, RemoteItem destination);
        
        /// <summary>
        /// Access the path given by name <br></br>
        /// Reload the client in the same time
        /// </summary>
        /// <param name="destination">the name folder or file</param>
        public abstract void AccessPath(RemoteItem destination);
        
        /// <summary>
        /// Refresh the children of the client
        /// </summary>
        public abstract void Refresh();

        /// <summary>
        /// Verify whether the item is in the folder
        /// </summary>
        /// <param name="folder">the folder</param>
        /// <param name="item">the given item to search</param>
        /// <returns></returns>
        public abstract bool ItemExist(RemoteItem folder, RemoteItem item);

        /// <summary>
        /// Get the item by its name if it exists in the current loaded folder
        /// </summary>
        /// <returns>The remote Item</returns>
        public abstract RemoteItem? GetItem(string name);

        #endregion

    }

    public enum ClientType
    {
        FTP,
        ONEDRIVE,
        GOOGLEDRIVE
    }
}