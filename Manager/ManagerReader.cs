using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using Manager.ManagerExceptions;

namespace Manager
{
    
    /// <summary>
    /// -- ManagerReader Class --<br></br>
    /// * Purpose : Create all methods and scripts for reading and saving purpose. <br></br>
    /// * Methods : Getter of Properties for files, Generating pointer using saver function, Basic algorithm for basic treatment of strings and lists <br></br>
    /// * Specifications : Low level and High level implementation have dependencies, you should read descriptions before doing modifications. <br></br>
    /// </summary>
    public static class ManagerReader
    {
        #region Properties

        // This region contains every function that give information of the file given with the path or pointer

        /// <summary>
        /// - Type : Low Level  <br></br>
        /// - Action : Verify whether a file or a directory is hidden or not  <br></br>
        /// - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>Whether it is hidden or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileHidden(string path)
        {
            return HasAttribute(FileAttributes.Hidden, path);
        }
        
        /// <summary>
        /// - Type : High Level  <br></br>
        /// -> <see cref="IsFileHidden(string)"/>
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>Whether it is hidden or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileHidden(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsFileHidden");
            return IsFileHidden(ft.Path);
        }


        /// <summary>
        /// _ Type : Low Level  <br></br>
        /// - Action : Verify whether a file or directory is compressed  <br></br>
        /// - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>Whether it is compressed or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileCompressed(string path)
        {
            return HasAttribute(FileAttributes.Compressed, path);
        }
        
        /// <summary>
        /// _ Type : High Level  <br></br>
        /// -> <see cref="IsFileCompressed(string)"/>
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>Whether it is compressed or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileCompressed(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsFileCompressed");
            return IsFileCompressed(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level  <br></br>
        /// - Action : Verify whether a file or a directory is archived  <br></br>
        /// - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>Whether it is archived or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileArchived(string path)
        {
            return HasAttribute(FileAttributes.Archive, path);
        }
        
        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="IsFileArchived(string)"/>
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>Whether it is archived or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileArchived(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsFileArchived");
            return IsFileArchived(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level  <br></br>
        /// - Action : Verify whether a file or directory is part of a the system  <br></br>
        /// - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>If the file/folder is part of system or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsASystemFile(string path)
        {
            return HasAttribute(FileAttributes.System, path);
        }
        
        /// <summary>
        /// - Type : High Level  <br></br>
        /// -> <see cref="IsASystemFile(string)"/>
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>If the file/folder is part of system or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsASystemFile(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsASystemFile");
            return IsASystemFile(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level  <br></br>
        /// - Action : Verify whether a file or directory is in readOnly  <br></br>
        /// - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>If the file is in readOnly</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsReadOnly(string path)
        {
            return HasAttribute(FileAttributes.ReadOnly, path);
        }
        
        /// <summary>
        /// - Type : High Level  <br></br>
        /// -> <see cref="IsReadOnly(string)"/>
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>If the file is in readOnly</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsReadOnly(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsReadOnly");
            return IsReadOnly(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level  <br></br>
        /// - Action : Verify whether the given file or directory has fa attribute <br></br>
        /// - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="fa">the attribute to test</param>
        /// <param name="path">the path to test</param>
        /// <returns></returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool HasAttribute(FileAttributes fa, string path)
        {
            // If it is a file : 
            if (File.Exists(path))
            {
                try
                {
                    return (File.GetAttributes(path) & fa) != 0;
                }
                catch (Exception e)
                {
                    if (e is IOException)
                        throw new InUseException("the file " + path + " is used by an external program",
                            "HasAttribute");
                    if (e is UnauthorizedAccessException)
                        throw new AccessException("the file " + path + " access is denied", "HasAttribute");
                    throw new ManagerException("Reader error", "High","Impossible to read","the given path " + path + " could not be read", "HasAttribute");
                }
            }
            // If it is a directory
            if (Directory.Exists(path))
            {
                try
                {
                    return (new DirectoryInfo(path).Attributes & fa) != 0;
                }
                catch (Exception e)
                {
                    if (e is SecurityException or UnauthorizedAccessException)
                        throw new AccessException("the directory to access # " + path + " # cannot be read",
                            "HasAttribute");
                    if (e is IOException)
                        throw new DiskNotReadyException("the given path " + path + " could not be read",
                            "HasAttribute");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        "the given path " + path + " could not be read", "HasAttribute");
                }
            }

            throw new PathNotFoundException(path + " does not exist", "HasAttribute");
        }

        #endregion

        #region Get

        // This region contains all Get function that also give information of files and directories

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action Get the parent directory full name <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the given path</param>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="PathNotFoundException">the file / folder does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetParent(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    return new FileInfo(path).DirectoryName;
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException or SecurityException)
                        throw new AccessException(path + " access denied", "GetParent");
                    throw new ManagerException("Access to parent","High","Parent not found","Error while trying to get the Directory of " + path, "GetParent");
                }
            }
            if (Directory.Exists(path))
            {
                try
                {
                    DirectoryInfo dir = new DirectoryInfo(path).Parent;
                    if (dir is null) return "";
                    return dir.FullName;

                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException or SecurityException)
                        throw new AccessException(path + " access denied", "GetParent");
                    throw new ManagerException("Access to parent","High","Parent not found","Error while trying to get the Directory of " + path, "GetParent");
                }
            }
            throw new PathNotFoundException(path + " does not exist", "GetParent");
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetParent(string)"/>
        /// </summary>
        /// <param name="ft">the given path (pointer)</param>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="PathNotFoundException">the file / folder does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetParent(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsFileHidden");
            return GetParent(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Get the date of creation with a given path <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the creation date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileCreationDate(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetCreationTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileCreationDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read", path + " could not be read", "GetFileCreationDate");
                }
            }
            if (File.Exists(path))
            {
                try
                {
                    return File.GetCreationTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed","GetFileCreationDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read", path + " could not be read", "GetFileCreationDate");
                }
            }

            throw new PathNotFoundException(path + " does not exist", "GetFileCreationDate");
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetFileCreationDate(string)"/>
        /// </summary>
        /// <param name="ft">the file or directory (pointer)</param>
        /// <returns>the creation date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="CorruptedPointerException">the pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileCreationDate(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetFileCreationDate");
            return GetFileCreationDate(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level
        /// - Action : Get the date of last edition with a given path <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileLastEdition(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetLastWriteTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed","GetFileLastEdition");
                    throw new ManagerException("Reader error", "High", "Impossible to read", path + " could not be read", "GetFileLastEdition");
                }
            }
            if (File.Exists(path))
            {
                try
                {
                    return File.GetLastWriteTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed","GetFileLastEdition");
                    throw new ManagerException("Reader error", "High", "Impossible to read", path + " could not be read", "GetFileLastEdition");
                }
            }

            throw new PathNotFoundException(path + " does not exist", "GetFileLastEdition");
        }

        /// <summary>
        /// - Type : High Level
        /// -> <see cref="GetFileCreationDate(string)"/>
        /// </summary>
        /// <param name="ft">the file or directory (pointer)</param>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="CorruptedPointerException">the pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileLastEdition(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetFileLastEdition");
            return GetFileLastEdition(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Get the date of access with a given path<br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileAccessDate(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return Directory.GetLastAccessTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed","GetFileAccessDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read", path + " could not be read", "GetFileAccessDate");
                }
            }
            if (File.Exists(path))
            {
                try
                {
                    return File.GetLastAccessTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed","GetFileAccessDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read", path + " could not be read", "GetFileAccessDate");
                }
            }

            throw new PathNotFoundException(path + " does not exist", "GetFileAccessDate");
        }
        
        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetFileAccessDate(string)"/>
        /// </summary>
        /// <param name="ft">the file or directory (pointer)</param>
        /// <returns>the last access date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="CorruptedPointerException">the pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileAccessDate(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetFileAccessDate");
            return GetFileAccessDate(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Get the file size in byte <br></br>
        /// - Implementation : Check (prototype)
        /// </summary>
        /// <returns>the size of the file or directory</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static long GetFileSize(string path)
        {
            if (Directory.Exists(path))
            {
                try
                {
                    return new DirectoryInfo(path).EnumerateFiles("*.*", SearchOption.AllDirectories)
                        .Sum(fi => fi.Length);
                }
                catch (Exception e)
                {
                    if (e is SecurityException)
                        throw new AccessException(path + " cannot be read", "GetFileSize");
                    throw new ManagerException("Reader error", "Medium", "Impossible to enumerate files", path + " and their children could not be read", "GetFileSize");
                }
            }
            if (File.Exists(path))
            {
                try
                {
                    return new FileInfo(path).Length;
                }
                catch (Exception e)
                {
                    if (e is SecurityException or UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be read", "GetFileSize");
                    if (e is IOException)
                        throw new DiskNotReadyException(path + " cannot be read", "GetFileSize");
                    throw new ManagerException("Reader error", "Medium", "Impossible to enumerate files", path + " and their children could not be read", "GetFileSize");
                }
            }
            throw new PathNotFoundException(path + " does not exist", "GetFileAccessDate");
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetFileSize(string)"/>
        /// </summary>
        /// <returns>the size of the file or directory</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupted</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static long GetFileSize(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetFileSize");
            return GetFileSize(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Reformat an absolute path to the name of the file or dir <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        public static string GetPathToName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetPathToName(string)"/>
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        public static string GetPathToName(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetPathToName");
            return GetPathToName(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Same as GetPathToName but does not give the extension of the file <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        public static string GetPathToNameNoExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetPathToNameNoExtension(string)"/>
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        /// <exception cref="CorruptedPointerException">the given pointer does not exist</exception>
        public static string GetPathToNameNoExtension(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetPathToNameNoExtension");
            return GetPathToNameNoExtension(ft.Path);
        }

        /// <summary>
        /// - Type : Low Level
        /// - Action : Reformat a name to the absolute path if the given name and current directory are correct <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>The full path of a file or directory</returns>
        /// <exception cref="AccessException">The given path cannot be read</exception>
        public static string GetNameToPath(string name)
        {
            try
            {
                string fullPath = Path.GetFullPath(name); // SecurityException, 
                fullPath = fullPath.Replace("\\", "/");
                return fullPath;
            }
            catch (Exception e)
            {
                if (e is SecurityException)
                    throw new AccessException(name + " cannot be read", "GetNameToPath");
                throw new ManagerException("Reader error", "Medium", "Transform absolute path to name", name + " could not be read", "GetNameToPath");
            }
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Returns the extension of a file. If it is a directory, it will return "" <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path to get extension of</param>
        /// <returns>Extension of a file</returns>
        public static string GetFileExtension(string path)
        {
            return Path.GetExtension(path);
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="GetFileExtension(string)"/>
        /// </summary>
        /// <param name="ft">the pointer that has to be identified</param>
        /// <returns>Extension of a file</returns>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupted</exception>
        public static string GetFileExtension(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "GetPathToNameNoExtension");
            return GetFileExtension(ft.Path);
        }

        #endregion

        #region Saver

        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI

        /// <summary>
        /// - Type : High Level, passed by string and generation of a pointer <br></br>
        /// - Action : Reads the properties of a File, modifies and inits its associated FileType <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path to save into a fileType</param>
        /// <returns>the file type associated</returns>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="Exception">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public static FileType ReadFileType(string path)
        {
            FileType ft = new FileType(path);
            ReadFileType(ref ft);
            return ft;
        }

        /// <summary>
        /// - Type : High Level, pointer passed by reference <br></br>
        /// - Action : Update FileType passed by reference <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <param name="ft">the fileType to update</param>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        /// <exception cref="PathNotFoundException">the data could not be found on the device</exception>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupted</exception>
        /// <exception cref="ManagerException">an unknown exception occured</exception>
        public static void ReadFileType(ref FileType ft)
        {
            if (Directory.Exists(ft.Path))
            {
                ft.Type = "";
                ft.IsDir = true;
                ft.Size = 0;
            }
            else
            {
                ft.Type = GetFileExtension(ft);
                ft.IsDir = false;
                ft.Size = GetFileSize(ft);
            }
            ft.Name = GetPathToName(ft);
            ft.ReadOnly = HasAttribute(FileAttributes.ReadOnly, ft.Path);
            ft.Hidden = HasAttribute(FileAttributes.Hidden, ft.Path);
            ft.Archived = HasAttribute(FileAttributes.Archive, ft.Path);
            ft.Date = GetFileCreationDate(ft);
            ft.LastDate = GetFileLastEdition(ft);
            ft.AccessDate = GetFileAccessDate(ft);
        }

        #endregion

        #region Algorithm

        #region Basics

        // This region contains every algorithm used for basic treatment

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : This function takes a path and generate a new path to avoid overwrite an existing file <br></br>
        /// - Implementation : Not Check <br></br>
        /// </summary>
        /// <param name="path">the path of the file/folder</param>
        /// <returns>Generate a file name</returns>
        /// <exception cref="PathNotFoundException">the data cannot be read</exception>
        /// <exception cref="AccessException">the file / folder or parent folder cannot be accessed</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GenerateNameForModification(string path)
        {
            // If the given path does not exist
            if (!File.Exists(path) && !Directory.Exists(path))
                throw new PathNotFoundException("the given path " + path + " does not exist",
                    "GenerateNameForModification");
            
            string res = path;
            string dir = GetParent(path); // Access Exception, PathNotFoundException, ManagerException
            int i = 0;
            // If it is a File, deal with extension
            if (File.Exists(path))
            {
                string name = GetPathToNameNoExtension(path);
                string extension = GetFileExtension(path);
                while (File.Exists(res) || Directory.Exists(res))
                {
                    i += 1;
                    res = $"{dir}/{name}({i}){extension}";
                }

                return res;
            }
            else
            {
                string name = GetPathToName(path);
                while (File.Exists(res) || Directory.Exists(res))
                {
                    i += 1;
                    res = $"{dir}/{name}({i})";
                }

                return res;
            }
        }

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : verify whether the path is correct <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path to test</param>
        /// <returns>correct or not</returns>
        public static bool IsPathCorrect(string path)
        {
            string name = GetPathToName(path);
            if (name.Length > 165 || path.Length > 255)
                return false;
            foreach (var c in Path.GetInvalidPathChars())
            {
                if (name.Contains(c))
                    return false;
            }
            return true;
        }

        public static bool IsPathCorrect(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "IsPathCorrect");
            return IsPathCorrect(ft.Path);
        }

        /// <summary>
        /// - Compares 2 strings and returns true if the first string is greater than the other one <br></br>
        /// - Implementation : Check <br></br>
        /// - Usage : Sort Algorithm
        /// </summary>
        private static bool GreaterThan(string s1, string s2)
        {
            int i = 0;
            int j = 0;
            int limit1 = s1.Count();
            int limit2 = s2.Count();
            while (i < limit1 && j < limit2)
            {
                if (s1[i] > s1[j])
                    return true;
                if (s2[j] > s1[i])
                    return false;
                i += 1;
                j += 1;
            }

            return i != limit1;
        }

        /// <summary>
        /// - Action: Verifies if the fist given date is greater than the second one <br></br>
        /// - Format : MONT/DAY/YEAR HOUR/MIN/SEC PM or AM <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>Returns if the first one is more recent than the other</returns>
        public static bool MoreRecentThanDate(string date1, string date2)
        {
            string[] date1List = date1.Split(' '); // [1/28/2022, 12:17:56, PM]
            string[] date2List = date2.Split(' ');
            if (date1List.Length != 3 || date1List.Length != 3)
                return false;

            string[] day1 = date1List[0].Split('/');
            string[] day2 = date2List[0].Split('/');
            string[] hour1 = date1List[1].Split(':');
            string[] hour2 = date2List[1].Split(':');

            if (day1.Length != 3 || day2.Length != 3)
                return false;

            // Compare dates
            for (int i = day1.Length - 1; i >= 0; i--)
            {
                if (int.Parse(day1[i]) > int.Parse(day2[i]))
                    return true;
                else if (int.Parse(day1[i]) < int.Parse(day2[i]))
                    return false;
            }

            // Compares Hours format
            string hourFormat = DateTime.Now.ToString(CultureInfo.CurrentCulture).Split(' ')[2];
            if (hourFormat == date1List[2] && hourFormat != date2List[2])
                return true;
            else if (hourFormat != date1List[2] && hourFormat == date2List[2])
                return false;

            // Comapares Hours
            string[] hour = DateTime.Now.ToString(CultureInfo.CurrentCulture).Split(' ')[1].Split(':');
            for (int i = hour.Length - 1; i >= 0; i--)
            {
                if (int.Parse(hour1[i]) > int.Parse(hour2[i]))
                    return true;
                if (int.Parse(hour1[i]) < int.Parse(hour2[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// - Action : Select FileTypes in a FileType list using a minimum size for their names <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <param name="fileTypes">List of fileType supposed to not be corrupted</param>
        /// <param name="minimumNameSize">the size of name required</param>
        /// <returns>the selected pointers with conditions</returns>
        private static List<FileType> SelectFileTypeByNameSize(List<FileType> fileTypes, int minimumNameSize)
        {
            List<FileType> res = new List<FileType>();
            foreach (FileType ft in fileTypes)
            {
                if (ft.Name.Count() >= minimumNameSize)
                    res.Add(ft);
            }

            return res;
        }

        #endregion

        #region Calculus

        // This region contains every function that deal with algorithm for calculus

        /// <summary>
        /// - Type : Low Level <br></br>
        /// - Action : Get the size and returns its value with a more relevant format <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <returns>Returns string for display</returns>
        public static string ByteToPowByte(long size)
        {
            if (size < 1024)
                return $"{size} B";
            if (size < 1048576)
                return $"{size / 1024} KB";
            if (size < 1073741824)
                return $"{size / 1048576} MB";
            return $"{size / 1073741824} GB";
        }

        /// <summary>
        /// - Type : High Level <br></br>
        /// -> <see cref="ByteToPowByte(long)"/>
        /// </summary>
        /// <returns>Returns string for display</returns>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupteds</exception>
        public static string ByteToPowByte(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed", "ByteToPowByte");
            return ByteToPowByte(ft.Size);
        }

        #endregion

        #region Sort

        // SORT BY STRINGS

        /// <summary>
        /// Create a new sorted list using merge algorithm
        /// This functions takes two FileType list sorted them using their NAMES and returns a new one sorted with all the elements
        /// Implementation : Check
        /// </summary>
        private static List<FileType> MergeSortFileTypeByName(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (GreaterThan(ftList1[i].Name, ftList2[j].Name))
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
                else
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            if (i != limit1)
            {
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            }
            else
            {
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            return ftReturned;
        }

        /// <summary>
        /// - Action : Create a new sorted list using merge algorithm <br></br>
        /// This functions takes two FileType list sorted them using their SIZES and returns a new one sorted with all the elements <br></br>
        ///- Implementation : Check <br></br>
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileTypeBySize(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (ftList1[i].Size > ftList2[j].Size)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
                else
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            if (i != limit1)
            {
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            }
            else
            {
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            return ftReturned;
        }

        /// <summary>
        /// - Action : Create a new sorted list using merge algorithm <br></br>
        /// This functions takes two FileType list sorted them using their TYPES and returns a new one sorted with all the elements <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileTypeByType(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (GreaterThan(ftList1[i].Type, ftList2[j].Type))
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
                else
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            if (i != limit1)
            {
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            }
            else
            {
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            return ftReturned;
        }

        /// <summary>
        /// - Action : Create a new sorted list using merge algorithm <br></br>
        /// This functions takes two FileType list sorted them using their MODIFIED DATES and returns a new one sorted with all the elements <br></br>
        /// Implementation : NOT Check
        /// </summary>
        private static List<FileType> MergeSortFileTypeByModifiedDate(List<FileType> ftList1, List<FileType> ftList2)
        {
            List<FileType> ftReturned = new List<FileType>();

            int i = 0;
            int j = 0;
            int limit1 = ftList1.Count;
            int limit2 = ftList2.Count;

            while (i < limit1 && j < limit2)
            {
                if (!MoreRecentThanDate(ftList1[i].Name, ftList2[j].Name))
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
                else
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            if (i != limit1)
            {
                while (i < limit1)
                {
                    ftReturned.Add(ftList1[i]);
                    i += 1;
                }
            }
            else
            {
                while (j < limit2)
                {
                    ftReturned.Add(ftList2[j]);
                    j += 1;
                }
            }

            return ftReturned;
        }

        // Sort functions

        /// <summary>
        /// - Action : MergeSort algorithm to sort files (by names) <br></br>
        /// - Implementation : Check (Working)
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByName(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "name");
        }

        /// <summary>
        /// - Action : MergeSort algorithm to sort files (by sizes) <br></br>
        /// - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortBySize(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "size");
        }

        /// <summary>
        /// - Action : MergeSort algorithm to sort files (by types) <br></br>
        /// - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>Returns the sorted list of filetype</returns>
        public static List<FileType> SortByType(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "type");
        }

        /// <summary>
        /// - Action : MergeSort algorithm to sort files (by modifiedDate) <br></br>
        /// - Implementation : Check (Working) <br></br>
        /// </summary>
        /// <param name="ftList">the lit of pointer to sort</param>
        /// <returns>the sorted list of filetype</returns>
        public static List<FileType>
            SortByModifiedDate(List<FileType> ftList)
        {
            return DivideAndMergeAlgorithm(ftList, "date");
        }

        // Main functions

        /// <summary>
        /// - Main algorithm : recursive method that divides and merge lists <br></br>
        /// - Action : This functions is used for sort algorithm because it is the most efficient algorithm for string treatment <br></br>
        /// - Implementation : Check <br></br>
        /// </summary>
        /// <returns>Returns the sorted pointer list</returns>
        private static List<FileType> DivideAndMergeAlgorithm(List<FileType> ftList, string argument)
        {
            // If list is empty, returns it
            if (ftList.Count() <= 1)
            {
                return ftList;
            }

            // If not empty divide them and call the function again
            List<FileType> ftList1 = new List<FileType>();
            List<FileType> ftList2 = new List<FileType>();
            for (int i = 0; i < ftList.Count / 2; i++)
                ftList1.Add(ftList[i]);
            for (int i = ftList.Count / 2; i < ftList.Count(); i++)
                ftList2.Add(ftList[i]);
            return MergeSortFileType(DivideAndMergeAlgorithm(ftList1, argument),
                DivideAndMergeAlgorithm(ftList2, argument), argument);
        }

        /// <summary>
        /// - Action : Sort fileType list using the merge sort algorithm <br></br>
        /// The string argument gets the wanted value to be sort <br></br>
        /// - Implementation : Not Check
        /// </summary>
        /// <returns></returns>
        private static List<FileType> MergeSortFileType(List<FileType> ftList1, List<FileType> ftList2, string argument)
        {
            switch (argument)
            {
                case "size":
                    return MergeSortFileTypeBySize(ftList1, ftList2);
                case "type":
                    return MergeSortFileTypeByType(ftList1, ftList2);
                case "name":
                    return MergeSortFileTypeByName(ftList1, ftList2);
                case "date":
                    return MergeSortFileTypeByModifiedDate(ftList1, ftList2);
                default:
                    return null;
            }
        }

        #endregion

        #region Search

        /// <summary>
        /// - Action : Naive research of a fileType <br></br>
        /// - Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find, null if it does not exists</returns>
        public static FileType SearchByFullName(List<FileType> fileTypes, string fullName)
        {
            foreach (FileType ft in fileTypes)
            {
                if (ft.Name == fullName)
                    return ft;
            }

            return SearchByIndeterminedName(fileTypes, fullName);
        }

        public static FileType SearchByFullName(DirectoryType directoryType, string fullName)
        {
            return SearchByFullName(directoryType.ChildrenFiles, fullName);
        }

        /// <summary>
        /// Naive research of a fileType using an indeterminatedName which will get the most relevant file <br></br>
        /// Implementation : Check
        /// </summary>
        /// <returns>return the fileType that has to be find</returns>
        public static FileType SearchByIndeterminedName(List<FileType> fileTypes, string indeterminedName)
        {
            if (fileTypes == null)
                return null;
            FileType bestFitft = fileTypes[0];
            int maxOcc = bestFitft.Name.Length;
            List<FileType> list = SelectFileTypeByNameSize(fileTypes, indeterminedName.Length);
            foreach (FileType ft in list)
            {
                if (ft.Name == indeterminedName)
                    return ft;
                else
                {
                    int currentOcc = 0;
                    while (currentOcc < indeterminedName.Length)
                    {
                        if (ft.Name[currentOcc] == indeterminedName[currentOcc])
                            currentOcc++;
                        else
                            break;
                    }

                    if (currentOcc == indeterminedName.Length)
                        return ft;
                    else if (currentOcc > maxOcc)
                    {
                        bestFitft = ft;
                        maxOcc = currentOcc;
                    }
                }
            }

            return bestFitft;
        }

        public static FileType SearchByIndeterminedName(DirectoryType directoryType, string fullName)
        {
            return SearchByIndeterminedName(directoryType.ChildrenFiles, fullName);
        }

        #endregion

        #endregion

        #region CommandLine

        // Implementation : Check
        public static void ReadContent(string path)
        {
            if (File.Exists(path))
            {
                Console.WriteLine(new StreamReader(path).ReadToEnd());
            }
        }

        #endregion
    }
}