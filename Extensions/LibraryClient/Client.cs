using Library;
using Library.DirectoryPointer;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Library.FilePointer;

namespace LibraryClient
{
    /// <summary>
    /// Abstract Client <br></br>
    /// - Purpose : Provide a whole interface to make interaction with the UI easier <br></br>
    /// - Used : GoogleDriveClient, OneDriveClient, FTPClient
    /// - Be aware that all access to a remote file or folder should be done by using its absolute path <br></br>
    /// </summary>
    public abstract class Client
    {
        public RemoteItem Root;
        
        public RemoteItem? CurrentFolder;
        public ClientType Type;
        public List<RemoteItem> Children;
        
        public Client(ClientType type)
        {
            // Attributes
            Type = type;
            Children = new List<RemoteItem>();
            CurrentFolder = Root;
            CurrentFolder = null;
        }

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
        /// <param name="folder">the folder of reference</param>
        /// <param name="name">the name of the file / folder to download</param>
        /// <returns>The </returns>
        public virtual Pointer Download(RemoteItem folder, string name)
        {
            
            if (File.Exists(name)) return new FilePointer(name);
            else return new DirectoryPointer(name);
        }
        /// <summary>
        /// Upload the Local Pointer to the RemoteFolder
        /// </summary>
        /// <param name="pointer">The Pointer</param>
        /// <param name="destination">The Remote Folder</param>
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
        /// Get the item by its name if it exists in the current loaded folder
        /// </summary>
        /// <param name="path">The path of the file, each file/folder separated by a /</param>
        /// <returns>The remote Item</returns>
        public abstract RemoteItem? GetItem(string path);

        /// <summary>
        /// Get the item by its current folder and its name
        /// </summary>
        /// <param name="folder">the current folder</param>
        /// <param name="name">the name of the file or folder</param>
        /// <returns>The remote item</returns>
        public abstract RemoteItem? GetItem(RemoteItem folder, string name);

        #endregion
        
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public abstract string GetItemName(RemoteItem item);
        public abstract string GetItemType(RemoteItem item);
        public abstract long GetItemSize(RemoteItem item);
        public abstract List<RemoteItem>? ListChildren(RemoteItem folder);

        public abstract void InitializeProperties(RemoteItem item);

        #endregion
        

    }

    public enum ClientType
    {
        FTP,
        ONEDRIVE,
        GOOGLEDRIVE
    }
}