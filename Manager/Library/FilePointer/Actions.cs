using System.Security;
using Library.ManagerExceptions;
using Microsoft.VisualBasic.FileIO;

namespace Library.FilePointer
{
    public partial class FilePointer : Pointer
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
            _fileInfo = new FileInfo(_path);
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
            // Dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(Path + " : format of path is incorrect", "Copy");
            if (!IsValid()) throw new PathNotFoundException(Path + " is invalid", "Copy");
            
            // Dest already exists and overwrite is activated : destroy it
            if (overwrite)
            {
                if (File.Exists(dest)) ManagerWriter.ManagerWriter.DeleteFile(dest);
                else if (Directory.Exists(dest)) ManagerWriter.ManagerWriter.DeleteDir(dest);
            }
            else
            {
                if (File.Exists(dest) || Directory.Exists(dest)) throw new ReplaceException($"Unable to rename {_path}", "RenameFile");
            }
            
            // Try to Rename
            if (!File.Exists(Path)) throw new PathNotFoundException($"{Path} does not exist", "RenameFile");
            if (File.Exists(dest) && !overwrite) throw new ReplaceException($"{dest} already exists, rename aborted", "RenameFile");
            try
            {
                FileSystem.RenameFile(_path, dest);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or ArgumentNullException or NotSupportedException or PathTooLongException => new PathFormatException($"{Path} has an invalid format", "RenameFile"),
                    FileNotFoundException => new PathNotFoundException($"{Path} could not be found in the client's system", "RenameFile"),
                    IOException => new DiskNotReadyException($"The disk is not ready to rename {dest}", "RenameFile"),
                    SecurityException or UnauthorizedAccessException => new AccessException($"Access to {Path} is denied", "RenameFile"),
                    _ => new ManagerException("Unable to rename a file", Level.High, "Writer error", $"Unable to rename {Path} to {dest}", "RenameFile")
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
        /// <param name="recursive">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public override FilePointer Copy(string dest, bool recursive = false)
        {
            // Source does not exist
            if (!Exist()) throw new PathNotFoundException($"{_path} not found", "Copy");
            // Source or dest have an incorrect format
            if (!IsValid() || !ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(_path + " : format of path is incorrect", "Copy");

            // Then finally Copy
            try
            {
                FileSystem.CopyFile(Path, dest, UIOption.AllDialogs, UICancelOption.DoNothing);
                return new FilePointer(dest);
            }
            catch (Exception e)
            {
                if (e is ManagerException) throw;
                throw e switch
                {
                    ArgumentException or ArgumentNullException => new PathFormatException($"{dest} has an invalid format", "Copy"),
                    UnauthorizedAccessException or SecurityException => new AccessException(Path + " could not be accessed", "Copy"),
                    IOException => new SystemErrorException("system blocked copy of " + Path + " to " + dest, "Copy"),
                    _ => new ManagerException("Writer Error", Level.Normal, "Unable to make a copy", $"The system was unable to make a copy of {_path}", "Copy")
                };
            }
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
                FileSystem.DeleteFile(_path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.DoNothing);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or ArgumentNullException or PathTooLongException => new PathFormatException(
                        $"The path {_path} is invalid","Delete file"),
                    FileNotFoundException or DirectoryNotFoundException => throw new PathNotFoundException(_path + " does not exist", "Delete"),
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
        ///     - Action : Add an attribute to a file pointer <br></br>
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
                _fileInfo!.Attributes |= attributes;
            }
            catch (Exception e)
            {
                throw e switch
                {
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
        ///     - Action : Remove a directory attributes  <br></br>
        ///     - Implementation : Check
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
                _fileInfo!.Attributes &= ~attributesToRemove;
            }
            catch (Exception e)
            {
                throw e switch
                {
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