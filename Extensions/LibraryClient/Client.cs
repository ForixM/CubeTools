﻿using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerExceptions;

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
        /// Delete a folder or a file in the current folder
        /// </summary>
        /// <param name="item">the item to delete</param>
        public abstract void Delete(RemoteItem item);
        
        /// <summary>
        /// Rename a folder or a file by the newName given <br></br>
        /// <remarks>The item is modified, not returned</remarks>
        /// </summary>
        /// <param name="item">the item to rename</param>
        /// <param name="newName">the name name of the item</param>
        /// <exception cref="ReplaceException">The newName given already exists</exception>
        public abstract void Rename(RemoteItem item, string newName);

        /// <summary>
        /// Download the file or folder given by its name (name)
        /// </summary>
        /// <param name="item">the file / folder to download</param>
        /// <param name="destination">The destination</param>
        /// <returns>The </returns>
        public virtual Pointer DownloadFile(RemoteItem item, DirectoryPointer destination) => new FilePointer(item.Name);
        
        /// <summary>
        /// Download the file or folder given by its name (name)
        /// </summary>
        /// <param name="item">the file / folder to download</param>
        /// <param name="destination">The destination</param>
        /// <returns>The downloaded folder</returns>
        public virtual Pointer DownloadFolder(RemoteItem item, DirectoryPointer destination) => new DirectoryPointer(item.Name);
        
        /// <summary>
        /// Upload the Local Pointer to the RemoteFolder
        /// </summary>
        /// <param name="pointer">The Pointer</param>
        /// <param name="destination">The Remote Folder</param>
        public abstract void UploadFile(FilePointer pointer, RemoteItem destination);
        
        /// <summary>
        /// Upload the Local Pointer to the RemoteFolder
        /// </summary>
        /// <param name="pointer">The Pointer</param>
        /// <param name="destination">The Remote Folder</param>
        public abstract void UploadFolder(DirectoryPointer pointer, RemoteItem destination);
        
        /// <summary>
        /// Access the path given by name <br></br>
        /// Reload the client in the same time
        /// </summary>
        /// <param name="destination">the name folder or file</param>
        public abstract void AccessPath(RemoteItem destination);

        /// <summary>
        /// Refresh the children of the client
        /// </summary>
        public void Refresh()
        {
            DisposeChildren();
            Children = ListChildren() ?? new List<RemoteItem>();
        }

        /// <summary>
        /// Get the item by its name if it exists in the current loaded folder
        /// </summary>
        /// <param name="path">The name of the file, each file/folder separated by a / in the current folder</param>
        /// <returns>The remote Item in the current folder</returns>
        public abstract RemoteItem? GetItem(string path, bool isAbsolute = false);

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
        public abstract RemoteItem GetParentReference(RemoteItem item);
        
        /// <summary>
        /// Return the children in the current folder <br></br>
        /// If the folder is null, return null
        /// </summary>
        /// <returns></returns>
        public abstract List<RemoteItem>? ListChildren();

        public abstract void InitializeProperties(RemoteItem item);

        #endregion
        
        #region Memory

        /// <summary>
        /// Dispose the children
        /// </summary>
        public void DisposeChildren()
        {
            foreach (var item in Children)
                item.Dispose();
            GC.Collect();
            Children.Clear();
        }
        
        #endregion
        

    }

    public enum ClientType
    {
        FTP,
        ONEDRIVE,
        GOOGLEDRIVE
    }
}