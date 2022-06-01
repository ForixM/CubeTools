﻿using System.Security;
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
            // Dest have an incorrect format
            if (!ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(Path + " : format of path is incorrect", "Copy");
            if (!IsValid())
            {
                Dispose();
                throw new PathNotFoundException(Path + " is invalid", "Copy");
            }
            
            // Dest already exists and overwrite is activated : destroy it
            if (overwrite)
            {
                if (File.Exists(dest)) ManagerWriter.ManagerWriter.DeleteFile(dest);
                else if (Directory.Exists(dest)) ManagerWriter.ManagerWriter.DeleteDir(dest);
            }
            // Try to Rename (move the directory)
            try
            {
                FileSystem.MoveDirectory(_path, dest, UIOption.AllDialogs, UICancelOption.ThrowException);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    SecurityException or UnauthorizedAccessException => new AccessException(
                        $"{_path} cannot be renamed, access denied", "Rename"),
                    ArgumentException or ArgumentNullException or PathTooLongException or NotSupportedException =>
                        new PathFormatException($"{_path} has an invalid format", "Rename"),
                    DirectoryNotFoundException => new PathNotFoundException($"{_path} cannot be found in the client's system", "Rename"),
                    _ => new ManagerException("", "", "", "", "Rename")
                };
            }
            
            // Update Information
            _path = dest;
            Update();
        }

        #endregion

        #region Copy

        /*
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
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public override DirectoryPointer Copy(string dest, bool replace = false)
        {
            // Source does not exist
            if (!Exist()) throw new PathNotFoundException($"{_path} not found", "Copy");
            // Source or dest have an incorrect format
            if (!IsValid() || !ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(_path + " : format of path is incorrect", "Copy");

            // Then finally Copy
            try
            {
                // Create the destination directory if it does not exist
                if (!Directory.Exists(dest)) ManagerWriter.ManagerWriter.CreateDir(dest);
                // Copy all sub files
                foreach (var file in _directoryInfo!.EnumerateFiles())
                {
                    if (replace && File.Exists(dest)) ManagerWriter.ManagerWriter.DeleteFile(dest);
                    file.CopyTo(System.IO.Path.Combine(dest, file.Name));
                }
                // Copy all sub folders
                foreach (var subDir in _directoryInfo.EnumerateDirectories())
                {
                    var newDestinationDir = System.IO.Path.Combine(dest, subDir.Name);
                    ManagerWriter.ManagerWriter.CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
                // Return the new DirectoryPointer
                return new DirectoryPointer(dest);
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
                    _ => new ManagerException("An error occured", "Medium", "Copy directories",
                        "Copy sub-dirs and sub-files could not be done", "CopyDirectory")
                };
            }
        }

        /// <summary>
        ///     - High Level Method : <see cref="Copy"/>
        /// </summary>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public override async Task<Pointer> CopyAsync(string dest, bool replace = false)
        {
            // Source does not exist
            if (!Exist()) throw new PathNotFoundException($"{_path} not found", "Copy");
            // Source or dest have an incorrect format
            if (!IsValid() || !ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(_path + " : format of path is incorrect", "Copy");

            return await new Task<Pointer>(() =>
            {
               // Then finally Copy
            try
            {
                // Create the destination directory if it does not exist
                if (!Directory.Exists(dest)) ManagerWriter.ManagerWriter.CreateDir(dest);
                
                // COPY OF FILES
                // Create tasks to copy all sub files
                foreach (var file in _directoryInfo!.EnumerateFiles())
                {
                    Thread thread = new(() =>
                    {
                        string newPath = System.IO.Path.Combine(dest, file.Name);
                        try
                        {
                            if (replace && File.Exists(dest)) ManagerWriter.ManagerWriter.DeleteFile(dest);
                            file.CopyTo(newPath);
                        }
                        catch (Exception)
                        {
                        }
                    });
                    thread.Priority = ThreadPriority.AboveNormal;
                    thread.Start();
                }

                // COPY OF FOLDERS
                // Copy all sub folders
                foreach (var subDir in _directoryInfo.EnumerateDirectories())
                {
                    Thread thread = new(() => ManagerWriter.ManagerWriter.CopyDirectory(subDir.FullName,
                            System.IO.Path.Combine(dest, subDir.Name), true))
                    {
                        Priority = ThreadPriority.AboveNormal
                    };
                    thread.Start();
                    thread.Join();
                }
                // Return the new DirectoryPointer
                return new DirectoryPointer(dest);
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
                    _ => new ManagerException("An error occured", "Medium", "Copy directories",
                        "Copy sub-dirs and sub-files could not be done", "CopyDirectory")
                };
            } 
            });
        }
        */

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
            if (!IsValid() || !ManagerReader.ManagerReader.IsPathCorrect(dest)) throw new PathFormatException(_path + " : format of path is incorrect", "Copy");

            // Then finally Copy
            try
            {
                FileSystem.CopyDirectory(_path, dest, UIOption.AllDialogs, UICancelOption.ThrowException);
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
                    _ => new ManagerException("An error occured", "Medium", "Copy directories",
                        "Copy sub-dirs and sub-files could not be done", "CopyDirectory")
                };
            }

            return new DirectoryPointer(dest);
        }

        /// <summary>
        ///     - High Level Method : <see cref="Copy"/>
        /// </summary>
        /// <param name="dest">the dest file or folder</param>
        /// <param name="replace">Replace a file or not : USE WITH PRECAUTION</param>
        /// <returns>Returns the new file created, empty for errors</returns>
        /// <exception cref="PathFormatException">The format of the path is incorrect</exception>
        /// <exception cref="PathNotFoundException">The given path could not be found</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="InUseException">The given path is already used in another process</exception>
        /// <exception cref="ManagerException">An error occured while copying</exception>
        public override async Task<Pointer> CopyAsync(string dest, bool replace = false) => await new Task<Pointer>(() => Copy(dest, replace));

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
                FileSystem.DeleteDirectory(_path, UIOption.AllDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new InUseException(_path + " is used by another process", "Delete"),
                    UnauthorizedAccessException => new AccessException(_path + " access is denied", "Delete"),
                    _ => new ManagerException("Delete is impossible", "Medium", "Writer Error",
                        _path + " could not be deleted", "Delete")
                };
            }
            Dispose();
        }

        /// <summary>
        ///   High Level Method : <see cref="Delete"/> 
        /// </summary>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public override async Task DeleteAsync() => await new Task(Delete);
        
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
                    FileNotFoundException or DirectoryNotFoundException => new PathNotFoundException(
                        "the given directory does not exist", "AddDirAttribute"),
                    UnauthorizedAccessException or SecurityException => new AccessException(
                        "The given directory cannot be accessed", "AddDirAttribute"),
                    IOException => new DiskNotReadyException("the disk is not ready to modify data", "AddDirAttribute"),
                    _ => new ManagerException("Writer error", "Medium", "Impossible to modify directory attributes",
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
                    FileNotFoundException or DirectoryNotFoundException => new PathNotFoundException(
                        "the given directory does not exist", "RemoveDirAttribute"),
                    UnauthorizedAccessException or SecurityException => new AccessException(
                        "The given directory cannot be accessed", "RemoveDirAttribute"),
                    IOException => new DiskNotReadyException("the disk is not ready to modify data",
                        "RemoveDirAttribute"),
                    _ => new ManagerException("Writer error", "Medium", "Impossible to modify directory attributes",
                        "The path cannot be modified", "RemoveDirAttribute")
                };
            }
        }

        #endregion

    }
}