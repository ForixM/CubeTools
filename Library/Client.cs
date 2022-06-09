using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerExceptions;

namespace Library
{
    /// <summary>
    /// Abstract Client <br></br>
    /// - Purpose : Provide a whole interface to make interaction with the UI easier <br></br>
    /// - Used : GoogleDriveClient, OneDriveClient, FTPClient
    /// - Be aware that all access to a remote file or folder should be done by using its absolute path <br></br>
    /// </summary>
    public abstract class Client
    {
        public Pointer Root;
        
        public Pointer? CurrentFolder;
        public ClientType Type;
        public List<Pointer> Children;
        
        public Client(ClientType type)
        {
            // Attributes
            Type = type;
            Children = new List<Pointer>();
            CurrentFolder = Root;
            CurrentFolder = null;
        }

        #region Actions

        /// <summary>
        /// Create a file in the current folder
        /// </summary>
        /// <param name="name">the name of this file with its extension</param>
        /// <returns>The remote item created</returns>
        public abstract Pointer CreateFile(string name);
        
        /// <summary>
        /// Create a folder in the current folder
        /// </summary>
        /// <param name="name">the name of the folder</param>
        /// <returns>the created folder</returns>
        public abstract Pointer CreateFolder(string name);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        public abstract Pointer? Copy(Pointer pointer);
        
        /// <summary>
        /// Delete a folder or a file in the current folder
        /// </summary>
        /// <param name="pointer">the item to delete</param>
        public abstract void Delete(Pointer pointer);
        
        /// <summary>
        /// Rename a folder or a file by the newName given <br></br>
        /// <remarks>The item is modified, not returned</remarks>
        /// </summary>
        /// <param name="pointer">the item to rename</param>
        /// <param name="newName">the name name of the item</param>
        /// <exception cref="ReplaceException">The newName given already exists</exception>
        public abstract void Rename(Pointer pointer, string newName);

        /// <summary>
        /// Download the file or folder given by its name (name)
        /// </summary>
        /// <param name="pointer">the file / folder to download</param>
        /// <param name="destination">The destination</param>
        /// <returns>The </returns>
        public virtual LocalPointer DownloadFile(Client source, Pointer pointer, Pointer destination) => new FileLocalPointer(pointer.Name);
        
        /// <summary>
        /// Download the file or folder given by its name (name)
        /// </summary>
        /// <param name="pointer">the file / folder to download</param>
        /// <param name="destination">The destination</param>
        /// <returns>The downloaded folder</returns>
        public virtual LocalPointer DownloadFolder(Client source, Pointer pointer, Pointer destination) => new DirectoryLocalPointer(pointer.Name);
        
        /// <summary>
        /// Upload the Local Pointer to the RemoteFolder
        /// </summary>
        /// <param name="localPointer">The Pointer</param>
        /// <param name="destination">The Remote Folder</param>
        public abstract void UploadFile(Client source, Pointer localPointer, Pointer destination);
        
        /// <summary>
        /// Upload the Local Pointer to the RemoteFolder
        /// </summary>
        /// <param name="localPointer">The Pointer</param>
        /// <param name="destination">The Remote Folder</param>
        public abstract void UploadFolder(Client source, Pointer localPointer, Pointer destination);

        public virtual Pointer? CompressFile(Pointer pointer) => null;
        
        /// <summary>
        /// Access the path given by name <br></br>
        /// Reload the client in the same time
        /// </summary>
        /// <param name="destination">the name folder or file</param>
        public abstract void AccessPath(Pointer destination);
        
        /// <summary>
        /// Get the item by its name if it exists in the current loaded folder
        /// </summary>
        /// <param name="path">The name of the file, each file/folder separated by a / in the current folder</param>
        /// <returns>The remote Item in the current folder</returns>
        public abstract Pointer? GetItem(string path, bool isAbsolute = false);

        #endregion
        
        #region Properties

        /// <summary>
        /// Get the name of the pointer and returns it
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The name of the pointer</returns>
        public abstract string GetItemName(Pointer pointer);
        /// <summary>
        /// Get the Type of data that is linked to the pointer
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The type of pointer</returns>
        public abstract string GetItemType(Pointer pointer);
        /// <summary>
        /// Calculate or Get the size of a pointer
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The size of this pointer</returns>
        public abstract long GetItemSize(Pointer pointer);
        /// <summary>
        /// Get the creation date of a file / folder of a pointer
        /// <remarks> This function should not be implemented if the client is unable to get the creation date</remarks>
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The creation data</returns>
        public virtual string GetItemCreationDate(Pointer pointer) => "";
        /// <summary>
        /// Get the last edition data of a file / folder linked to the pointer
        /// <remarks> This function should not be implemented if the client is unable to get the edition date</remarks>
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The edition date</returns>
        public virtual string GetItemLastEditionDate(Pointer pointer) => "";

        /// <summary>
        /// Get the access date of a data, file / folder, linked to the pointer
        /// <remarks> This function should not be implemented if the client is unable to get the access date</remarks>
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The access date</returns>
        public virtual string GetItemAccessDate(Pointer pointer) => "";

        /// <summary>
        /// Check if the pointer is in read only mode
        /// <remarks> Should not be implemented for remote client</remarks>
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>Whether it is in read only mode</returns>
        public virtual bool GetItemReadOnlyProperty(Pointer pointer) => false;
        /// <summary>
        /// Check if the pointer is in hidden mode
        /// <remarks> Should not be implemented for remote client</remarks>
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>Whether it is in read only mode</returns>
        public virtual bool GetItemHiddenProperty(Pointer pointer) => false;
        /// <summary>
        /// Set the read-only property
        /// </summary>
        /// <param name="pointer">The given pointer to modify</param>
        /// <param name="set">Whether the read only property has to be set</param>
        public virtual void SetReadOnlyProperty(Pointer pointer, bool set) { }

        /// <summary>
        /// Set the hidden property to a file
        /// </summary>
        /// <param name="pointer">The given pointer to modify</param>
        /// <param name="set">Whether the file / folder has to have the property</param>
        public virtual void SetHiddenProperty(Pointer pointer, bool set) { }
        /// <summary>
        /// Get the reference to the parent of the given pointer <br></br>
        /// <remarks> If the pointer has no pointer, returns itself</remarks>
        /// </summary>
        /// <param name="pointer">The given pointer passed by reference</param>
        /// <returns>The parent reference</returns>
        public abstract Pointer GetParentReference(Pointer pointer);
        
        /// <summary>
        /// Return the children in the current folder <br></br>
        /// If the folder is null, return null
        /// </summary>
        /// <returns></returns>
        public abstract List<Pointer>? ListChildren();

        public abstract void InitializeProperties(Pointer pointer);

        #endregion
        
        #region Memory
        
        /// <summary>
        /// Refresh the children of the client
        /// </summary>
        public void Refresh()
        {
            DisposeChildren();
            Children = ListChildren() ?? new List<Pointer>();
        }

        /// <summary>
        /// Dispose the children
        /// </summary>
        protected void DisposeChildren()
        {
            foreach (var item in Children) item.Dispose();
            GC.Collect();
            Children.Clear();
        }
        
        #endregion
        

    }

    public enum ClientType
    {
        FTP,
        ONEDRIVE,
        GOOGLEDRIVE,
        LOCAL
    }
}