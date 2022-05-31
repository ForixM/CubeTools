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

        public override void Update()
        {
            _fileInfo = new FileInfo(_path);
            
        }

        #endregion
        #region Rename

        public void Rename(string dest, bool overwrite = false)
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
            
            // Try to Rename
            if (!File.Exists(Path)) throw new PathNotFoundException($"{Path} does not exist", "RenameFile");
            if (File.Exists(dest) && !overwrite) throw new ReplaceException($"{dest} already exists, rename aborted", "RenameFile");
            try
            {
                File.Move(Path, dest, overwrite);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    ArgumentException or ArgumentNullException or NotSupportedException or PathTooLongException => new PathFormatException($"{Path} has an invalid format", "RenameFile"),
                    FileNotFoundException => new PathNotFoundException($"{Path} could not be found in the client's system", "RenameFile"),
                    IOException => new DiskNotReadyException($"The disk is not ready to rename {dest}", "RenameFile"),
                    SecurityException or UnauthorizedAccessException => new AccessException($"Access to {Path} is denied", "RenameFile"),
                    _ => new ManagerException("Unable to rename a file", "High", "Writer error", $"Unable to rename {Path} to {dest}", "RenameFile")
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
        ///     - Action : Copy file pointer and create a new one (or replace it) to dest given <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public Pointer Copy(string dest, bool replace = false)
        {
            // Dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(Path + " : format of path is incorrect", "Copy");
            if (!IsValid()) throw new PathNotFoundException(Path + " is invalid", "Copy");
            
            // Dest already exists and replacement is activated : destroy it
            if (replace)
            {
                if (File.Exists(dest))
                {
                    try
                    {
                        ManagerWriter.ManagerWriter.DeleteFile(dest);
                    }
                    catch (Exception e)
                    {
                        throw e switch
                        {
                            UnauthorizedAccessException => new AccessException(dest + " could not be replaced", "Copy"),
                            IOException => new InUseException(dest + " could not be replaced", "Copy"),
                            _ => new ManagerException("An error occured", "High", "Impossible to replace",
                                "Copy was impossible, replace could not be done", "Copy")
                        };
                    }
                }
                else if (Directory.Exists(dest))
                {
                    try
                    {
                        ManagerWriter.ManagerWriter.DeleteDir(dest);
                    }
                    catch (Exception e)
                    {
                        if (e is UnauthorizedAccessException)
                            throw new AccessException(dest + " could not be replaced", "Copy");
                        throw new ManagerException("An error occured", "Medium", "Impossible to replace",
                            "Copy was impossible, replace could not be done", "Copy");
                    }
                }
            }

            // Then finally Copy
            try
            {
                File.Copy(Path, dest);
                return new FilePointer(dest);
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case ManagerException:
                        throw;
                    case UnauthorizedAccessException or SecurityException:
                        throw new AccessException(Path + " could not be accessed", "Copy");
                    case IOException:
                        throw new SystemErrorException("system blocked copy of " + Path + " to " + dest, "Copy");
                    default:
                        throw new ManagerException("", "", "", "", "Copy");
                }
            }
        }

        /// <summary>
        /// Async version of the previous <see cref="Copy"/>
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        /// <exception cref="PathFormatException"></exception>
        /// <exception cref="PathNotFoundException"></exception>
        /// <exception cref="ManagerException"></exception>
        /// <exception cref="AccessException"></exception>
        /// <exception cref="SystemErrorException"></exception>
        public Task<Pointer> CopyAsync(string dest, bool replace = false)
        {
            // Dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(Path + " : format of path is incorrect", "Copy");
            if (!IsValid()) throw new PathNotFoundException(Path + " is invalid", "Copy");
            
            // Dest already exists and replacement is activated : destroy it
            if (replace)
            {
                try
                {
                    ManagerWriter.ManagerWriter.DeleteAsync(dest);
                }
                catch (Exception e)
                {
                    throw e switch
                    {
                        UnauthorizedAccessException => new AccessException(dest + " could not be replaced", "Copy"),
                        IOException => new InUseException(dest + " could not be replaced", "Copy"),
                        _ => new ManagerException("An error occured", "High", "Impossible to replace",
                            "Copy was impossible, replace could not be done", "Copy")
                    };
                }
            }
            // Finally Return the Task to Run
            return new Task<Pointer>(() =>
            {
                try
                {
                    FileSystem.CopyFile(Path, dest, UIOption.AllDialogs, UICancelOption.ThrowException);
                    return new FilePointer(dest);
                }
                catch (Exception e)
                {
                    switch (e)
                    {
                        case ManagerException:
                            throw;
                        case UnauthorizedAccessException or SecurityException:
                            throw new AccessException(Path + " could not be accessed", "Copy");
                        case IOException:
                            throw new SystemErrorException("system blocked copy of " + Path + " to " + dest, "Copy");
                        default:
                            throw new ManagerException("", "", "", "", "Copy");
                    }
                }
            });
        }

        #endregion
        
        #region Delete

        /// <summary>
        ///     - High Level => Delete a file using its associated FilePointer <br></br>
        ///     - Action : Delete a file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public void Delete()
        {
            if (!Exist()) throw new PathNotFoundException(Path + " does not exist", "Delete");
            try
            {
                FileSystem.DeleteFile(Path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new InUseException(Path + " is used by another process", "Delete"),
                    UnauthorizedAccessException => new AccessException(Path + " access is denied", "Delete"),
                    _ => new ManagerException("Delete is impossible", "Medium", "Writer Error",
                        Path + " could not be deleted", "Delete")
                };
            }
        }
        
        #endregion

    }
}