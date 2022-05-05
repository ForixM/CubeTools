// System's import

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using Library.ManagerExceptions;
using Library.Pointers;

// Library's imports

namespace Library.ManagerReader
{
    /// <summary>
    ///     -- ManagerReader Class --<br></br>
    ///     * Purpose : Create all methods and scripts for reading and saving purpose. <br></br>
    ///     * Methods : Getter of Properties for files, Generating pointer using saver function, Basic algorithm for basic
    ///     treatment of strings and lists <br></br>
    ///     * Specifications : Low level and High level implementation have dependencies, you should read descriptions before
    ///     doing modifications. <br></br>
    /// </summary>
    public static partial class ManagerReader
    {
        // This region contains all Get function that also give information of files and directories
        // Reader function useful for little treatment

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action Get the parent directory full name <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the given path</param>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="PathNotFoundException">the file / folder does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetParent(string path)
        {
            if (File.Exists(path))
                try
                {
                    return new FileInfo(path).DirectoryName;
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException or SecurityException)
                        throw new AccessException(path + " access denied", "GetParent");
                    throw new ManagerException("Access to parent", "High", "Parent not found",
                        "Error while trying to get the Directory of " + path, "GetParent");
                }

            if (Directory.Exists(path))
                try
                {
                    var dir = new DirectoryInfo(path).Parent;
                    if (dir is null) return "";
                    return dir.FullName;
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException or SecurityException)
                        throw new AccessException(path + " access denied", "GetParent");
                    throw new ManagerException("Access to parent", "High", "Parent not found",
                        "Error while trying to get the Directory of " + path, "GetParent");
                }

            throw new PathNotFoundException(path + " does not exist", "GetParent");
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetParent(string)" />
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
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsFileHidden");
            return GetParent(ft.Path);
        }

        /// <summary>
        ///     - Action : using Path class, it generates the root's path of a given path
        ///     - Implementation : NOT CHECK
        /// </summary>
        /// <param name="path">The given path</param>
        /// <returns>Returns the root's path of the given path</returns>
        /// <exception cref="PathFormatException">The given path has an incorrect format</exception>
        public static string GetRootPath(string path)
        {
            try
            {
                return Path.GetPathRoot(path);
            }
            catch (ArgumentException)
            {
                throw new PathFormatException("The given format of path is invalid", "GetRootPath");
            }
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Get the date of creation with a given path <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the creation date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileCreationDate(string path)
        {
            if (Directory.Exists(path))
                try
                {
                    return Directory.GetCreationTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileCreationDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        path + " could not be read", "GetFileCreationDate");
                }

            if (File.Exists(path))
                try
                {
                    return File.GetCreationTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileCreationDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        path + " could not be read", "GetFileCreationDate");
                }

            throw new PathNotFoundException(path + " does not exist", "GetFileCreationDate");
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetFileCreationDate(string)" />
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
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetFileCreationDate");
            return GetFileCreationDate(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level
        ///     - Action : Get the date of last edition with a given path <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileLastEdition(string path)
        {
            if (Directory.Exists(path))
                try
                {
                    return Directory.GetLastWriteTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileLastEdition");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        path + " could not be read", "GetFileLastEdition");
                }

            if (File.Exists(path))
                try
                {
                    return File.GetLastWriteTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileLastEdition");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        path + " could not be read", "GetFileLastEdition");
                }

            throw new PathNotFoundException(path + " does not exist", "GetFileLastEdition");
        }

        /// <summary>
        ///     - Type : High Level
        ///     -> <see cref="GetFileCreationDate(string)" />
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
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetFileLastEdition");
            return GetFileLastEdition(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Get the date of access with a given path<br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <param name="path">the file or directory</param>
        /// <returns>the last edition date</returns>
        /// <exception cref="AccessException">the path cannot be read</exception>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetFileAccessDate(string path)
        {
            if (Directory.Exists(path))
                try
                {
                    return Directory.GetLastAccessTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileAccessDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        path + " could not be read", "GetFileAccessDate");
                }

            if (File.Exists(path))
                try
                {
                    return File.GetLastAccessTime(path).ToString(CultureInfo.CurrentCulture);
                }
                catch (Exception e)
                {
                    if (e is UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be accessed", "GetFileAccessDate");
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        path + " could not be read", "GetFileAccessDate");
                }

            throw new PathNotFoundException(path + " does not exist", "GetFileAccessDate");
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetFileAccessDate(string)" />
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
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetFileAccessDate");
            return GetFileAccessDate(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Get the file size in byte <br></br>
        ///     - Implementation : Check (prototype)
        /// </summary>
        /// <returns>the size of the file or directory</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static long GetFileSize(string path)
        {
            if (Directory.Exists(path))
                try
                {
                    return new DirectoryInfo(path).EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
                }
                catch (Exception e)
                {
                    if (e is SecurityException or UnauthorizedAccessException)
                        throw new AccessException(path + " cannot be read", "GetFileSize");
                    throw new ManagerException("Reader error", "Medium", "Impossible to enumerate files",
                        path + " and their children could not be read", "GetFileSize");
                }
            if (File.Exists(path))
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
                    throw new ManagerException("Reader error", "Medium", "Impossible to enumerate files",
                        path + " and their children could not be read", "GetFileSize");
                }
            throw new PathNotFoundException(path + " does not exist", "GetFileAccessDate");
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetFileSize(string)" />
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
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetFileSize");
            return GetFileSize(ft.Path);
        }
        
        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Reformat an absolute path to the name of the file or dir <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        public static string GetPathToName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetPathToName(string)" />
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        /// <exception cref="CorruptedPointerException">The given pointer is corrupted</exception>
        public static string GetPathToName(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetPathToName");
            return GetPathToName(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Same as GetPathToName but does not give the extension of the file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        public static string GetPathToNameNoExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetPathToNameNoExtension(string)" />
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        /// <exception cref="CorruptedPointerException">the given pointer does not exist</exception>
        public static string GetPathToNameNoExtension(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetPathToNameNoExtension");
            return GetPathToNameNoExtension(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Reformat a name to the absolute path if the given name and current directory are correct <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>The full path of a file or directory</returns>
        /// <exception cref="AccessException">The given path cannot be read</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetNameToPath(string name)
        {
            try
            {
                var fullPath = Path.GetFullPath(name); // SecurityException, 
                fullPath = fullPath.Replace("\\", "/");
                return fullPath;
            }
            catch (Exception e)
            {
                if (e is SecurityException)
                    throw new AccessException(name + " cannot be read", "GetNameToPath");
                throw new ManagerException("Reader error", "Medium", "Transform absolute path to name",
                    name + " could not be read", "GetNameToPath");
            }
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Returns the extension of a file. If it is a directory, it will return "" <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path to get extension of</param>
        /// <returns>Extension of a file</returns>
        public static string GetFileExtension(string path)
        {
            string extension = Path.GetExtension(path);
            if (extension.Length == 0)
                return extension;
            return extension.Remove(0, 1);
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="GetFileExtension(string)" />
        /// </summary>
        /// <param name="ft">the pointer that has to be identified</param>
        /// <returns>Extension of a file</returns>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupted</exception>
        public static string GetFileExtension(FileType ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "GetPathToNameNoExtension");
            return GetFileExtension(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Read a file and returns its content <br></br>
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <returns>the content</returns>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="PathFormatException">the format of the path is incorrect</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static string GetContent(string path)
        {
            if (!File.Exists(path))
                throw new PathNotFoundException(path + " does not exist", "GetContent");
            try
            {
                var sr = new StreamReader(path);
                var res = sr.ReadToEnd();
                sr.Close();
                return res;
            }
            catch (Exception e)
            {
                if (e is IOException)
                    throw new PathFormatException(path + " format is incorrect", "GetContent");
                if (e is OutOfMemoryException)
                {
                    Console.Error.WriteLine(path + " is too large to be read");
                    return "";
                }

                throw new ManagerException("Reader Error", "Medium", "Content not readable",
                    "Content could not be read", "GetContent");
            }
        }

        /// <summary>
        /// Basic return of the current Platform
        /// </summary>
        /// <returns></returns>
        public static PlatformID GetOs()
        {
            return Environment.OSVersion.Platform;
        }

        /// <summary>
        /// - Action : Returns the amount of sub-files and sub-dirs (NOT RECURSIVE)
        /// - Implementation : NOT CHECK 
        /// </summary>
        /// <param name="path">The given DIRECTORY's path that has to be analyzed</param>
        /// <returns></returns>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="ManagerException">An unknown error occured</exception>
        public static int GetAmountOfLocalData(string path)
        {
            if (!Directory.Exists(path))
                throw new PathNotFoundException("", ""); // TODO Add exception
            try
            {
                DirectoryInfo di = new(path); 
                return di.EnumerateDirectories().Count() + di.EnumerateFiles().Count();
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException("", ""); // TODO Add expcetion
                throw new ManagerException("", "", "", "", "");
            }
        }
    }
}