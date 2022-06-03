using System.Security;
using Library.ManagerExceptions;

namespace Library
{
    public abstract partial class Pointer
    {
        // This region contains elementary actions
        // Purpose : transform static library functions into methods of the class FilePointer
        // Abstract and virtual method

        // Every methods that update the pointer
        #region Update

        public virtual void Update()
        {
            // Necessary
            _name = ManagerReader.ManagerReader.GetPathToName(_path);
            _size = _isSizeLoaded ? ManagerReader.ManagerReader.GetFileSize(_path) : 0;
            _type = ManagerReader.ManagerReader.GetFileExtension(_path);
        }

        #endregion
        
        // Every methods that rename the pointer
        #region Rename
        
        /// <summary>
        /// Rename the pointer
        /// </summary>
        /// <param name="dest">the destination path</param>
        /// <param name="overwrite">Whether it has to overwrite if dest already exists</param>
        public abstract void Rename(string dest, bool overwrite = false);

        /// <summary>
        /// Async version of <see cref="Rename"/>
        /// </summary>
        /// <param name="dest">the destination path</param>
        /// <param name="overwrite">Whether it has to overwrite if dest already exists</param>
        public virtual async Task RenameAsync(string dest, bool overwrite = false) => await Task.Run(() => Rename(dest, overwrite));

        #endregion

        // Every methods that generate a copy of the pointer
        #region Copy

        /// <summary>
        ///     - High Level Method : Copy the content in the source file or folder into the dest file or folder <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="recursive">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public abstract Pointer Copy(string dest, bool recursive = false);

        /// <summary>
        /// Async version of the previous <see cref="Copy"/>
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="replace"></param>
        /// <returns>The Task to execute which will return the Pointer generated</returns>
        /// <exception cref="PathFormatException"></exception>
        /// <exception cref="PathNotFoundException"></exception>
        /// <exception cref="ManagerException"></exception>
        /// <exception cref="AccessException"></exception>
        /// <exception cref="SystemErrorException"></exception>
        public virtual async Task<Pointer> CopyAsync(string dest, bool replace = false) => await Task.Run(() => Copy(dest, replace));

        #endregion
        
        // Every methods that destroy the pointer
        #region Delete

        /// <summary>
        ///     - High Level Method => Delete a file using its associated pointer <br></br>
        ///     - Action : Delete a file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public abstract void Delete();


        /// <summary>
        ///   High Level Method : <see cref="Delete"/>
        /// </summary>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public abstract Task DeleteAsync();

        #endregion
        
        // Every methods needed to set properties information on a pointer
        #region Properties
        

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Add an attribute to a pointer <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="attributes">attributes to add </param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public abstract void AddAttribute(FileAttributes attributes);

        /// <summary>
        ///     - Action : Remove an attribute to the pointer  <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="attributesToRemove">the attribute to remove</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public abstract void RemoveAttribute(FileAttributes attributesToRemove);
        
        
        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Set an attribute given in parameter to a pointer <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="set">whether it has to be set or not</param>
        /// <param name="fa">the attribute that has to be set or not</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public void SetAttributes(bool set, FileAttributes fa)
        {
            try
            {
                if (set) AddAttribute(fa);
                else RemoveAttribute(fa);
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException("the file given " + _path + " access is denied", "SetAttributes");
                throw new ManagerException();
            }
        }
        
        
        #endregion

    }
}