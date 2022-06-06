using System.Security;
using Library.ManagerExceptions;
using Microsoft.VisualBasic.FileIO;

namespace Library.DirectoryPointer
{
    public partial class DirectoryPointer : Pointer
    {
        // This region contains elementary actions
        // Purpose : transform static library functions into methods of the class FilePointer

        #region Update

        /// <summary>
        /// Specific to the FilePointer : Update the FilePointer
        /// </summary>
        public override void Update()
        {
            base.Update();
            _directoryInfo = new DirectoryInfo(_path);
        }

        #endregion
        
        #region Rename

        /// <summary>
        ///     - High Level Method : Rename the pointer<br></br>
        ///     - Action : Copy file pointer and create a new one (or recursive it) to dest given <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="dest">the destination name</param>
        /// <param name="overwrite">Whether the pointer has to overwrite the data</param>
        /// <exception cref="PathFormatException">The destination has an invalid format</exception>
        /// <exception cref="PathNotFoundException">The pointer cannot be found in the system</exception>
        /// <exception cref="ReplaceException"></exception>
        /// <exception cref="ManagerException"></exception>
        public override void Rename(string dest, bool overwrite = false)
        {
            // Dest has an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(Path + " : format of path is incorrect", "Copy");
            if (!IsValid()) throw new PathNotFoundException(Path + " is invalid", "Copy");
            
            // Dest already exists and overwrite is activated : destroy it
            if (overwrite)
            {
                try
                {
                    if (File.Exists(dest)) ManagerWriter.ManagerWriter.DeleteFile(dest);
                    else if (Directory.Exists(dest)) ManagerWriter.ManagerWriter.DeleteDir(dest);
                }
                catch (Exception e)
                { }
            }
            else
            {
                if (Directory.Exists(dest) || File.Exists(dest))
                    throw new ReplaceException($"{dest} already exist", "Rename");
            }
            
            // Try to Rename the directory
            try
            {
                FileSystem.RenameDirectory(_path, dest);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    DirectoryNotFoundException => new PathNotFoundException($"{_path} cannot be found in the client's system", "Rename"),
                    SecurityException or UnauthorizedAccessException or IOException => new AccessException(
                        $"{_path} cannot be renamed, access denied", "Rename"),
                    ArgumentException or ArgumentNullException or PathTooLongException or NotSupportedException =>
                        new PathFormatException($"{_path} has an invalid format", "Rename"),
                    _ => new ManagerException("Unable to rename a folder", Level.High, "Writer error", $"Cannot rename {_path}", "Rename"),
                };
            }
            
            // Update Information
            _path = dest;
            Update();
        }

        #endregion

        #region Copy

        /// <summary>
        ///     - High Level Method : Copy the content in the source file or folder into the dest file or folder <br></br>
        ///     - Action : Copy file pointer and create a new one (or recursive it) to dest given <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace"></param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying, the user could have cancel for instance</exception>
        public override Pointer Copy(string dest, bool replace = false)
        {
            // Source does not exist
            if (!Exist()) throw new PathNotFoundException($"{_path} not found", "Copy");
            // Source or dest have an incorrect format
            if (!IsValid()) throw new PathFormatException(_path + " : format of path is incorrect", "Copy");

            // Then finally Copy
            try
            {
                FileSystem.CopyDirectory(_path, dest, UIOption.AllDialogs, UICancelOption.DoNothing);
            }
            catch (Exception e)
            {
                if (e is ManagerException) throw;
                throw e switch
                {
                    SecurityException or UnauthorizedAccessException => new AccessException(
                        _path + " could not be accessed", "CopyDirectory"),
                    IOException => new SystemErrorException("Copy could not be done", "CopyDirectory"),
                    PathTooLongException or NotSupportedException => new PathFormatException(
                        "A file / folder contained a path too large", "CopyDirectory"),
                    _ => new ManagerException("An error occured", Level.Normal, "Copy directories",
                        "Copy sub-dirs and sub-files could not be done", "CopyDirectory")
                };
            }
            return new DirectoryPointer(dest);
        }
        
        #endregion
        
        #region Delete

        /// <summary>
        ///     - Type : High Level Method  <br></br>
        ///     - Action : Delete a file pointer <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public override void Delete()
        {
            if (!Exist()) throw new PathNotFoundException(_path + " does not exist", "Delete");
            try
            {
                FileSystem.DeleteDirectory(_path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
            }
            catch (Exception e)
            {
                if (e is ManagerException) throw;
                throw e switch
                {
                    IOException => new InUseException(_path + " is used by another process", "Delete"),
                    UnauthorizedAccessException => new AccessException(_path + " access is denied", "Delete"),
                    _ => new ManagerException("Delete is impossible", Level.Normal, "Writer Error",
                        _path + " could not be deleted", "Delete")
                };
            }
            Dispose();
            GC.Collect();
        }
        
        #endregion
        
        #region Properties

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Add an attribute to a directory pointer <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="attributes">attributes to add </param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override void AddAttribute(FileAttributes attributes)
        {
            try
            {
                _directoryInfo!.Attributes |= attributes;
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or PathTooLongException => new PathFormatException(
                        "the given folder has an invalid path", "AddAttribute"),
                    FileNotFoundException or DirectoryNotFoundException => new PathNotFoundException(
                        "the given directory does not exist", "AddDirAttribute"),
                    UnauthorizedAccessException or SecurityException => new AccessException(
                        "The given directory cannot be accessed", "AddDirAttribute"),
                    IOException => new DiskNotReadyException("the disk is not ready to modify data", "AddDirAttribute"),
                    _ => new ManagerException("Writer error", Level.Normal, "Impossible to modify directory attributes",
                        "The path cannot be modified", "AddDirAttribute")
                };
            }
        }

        /// <summary>
        ///     - High Level Method <br></br>
        ///     - Action : Remove an attribute to a directory pointer <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="attributesToRemove">the attribute to remove</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override void RemoveAttribute(FileAttributes attributesToRemove)
        {
            try
            {
                _directoryInfo!.Attributes &= ~attributesToRemove;
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or PathTooLongException => new PathFormatException(
                        "the given directory has an invalid path","RemoveDirAttribute"),
                    FileNotFoundException or DirectoryNotFoundException => new PathNotFoundException(
                        "the given directory does not exist", "RemoveDirAttribute"),
                    UnauthorizedAccessException or SecurityException => new AccessException(
                        "The given directory cannot be accessed", "RemoveDirAttribute"),
                    IOException => new DiskNotReadyException("the disk is not ready to modify data",
                        "RemoveDirAttribute"),
                    _ => new ManagerException("Writer error", Level.Normal, "Impossible to modify directory attributes",
                        "The path cannot be modified", "RemoveDirAttribute")
                };
            }
        }

        #endregion

    }
}