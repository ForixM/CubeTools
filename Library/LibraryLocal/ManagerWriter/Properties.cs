using System.Security;
using Library.ManagerExceptions;

namespace Library.ManagerWriter
{
    public static partial class ManagerWriter
    {
        #region Properties

        // This region contains every function that set information of the file given with the path

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Remove an attribute <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="fi">FileInfo instance of the given file</param>
        /// <param name="attributesToRemove">the attribute to remove</param>
        /// <returns>the new file attributes</returns>
        /// <exception cref="AccessException">the file cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void RemoveFileAttribute(FileInfo fi, FileAttributes attributesToRemove)
        {
            try
            {
                fi.Attributes &= ~attributesToRemove;
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException("File could not be accessed", "RemoveFileAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("Disk is refreshing", "RemoveFileAttribute");
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("The given file does not exist", "RemoveFileAttribute");
                throw new ManagerException("Reader error", Level.Normal, "Impossible to read file",
                    "The file cannot be read", "RemoveFileAttribute");
            }
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Add an attribute to a file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path that has to be modified</param>
        /// <param name="attributes">attributes to add </param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void AddFileAttribute(string path, FileAttributes attributes)
        {
            try
            {
                File.SetAttributes(path, attributes);
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException("File could not be accessed", "AddFileAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("Disk is refreshing", "AddFileAttribute");
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("The given file does not exist", "AddFileAttribute");
                throw new ManagerException("Writer error", Level.Normal, "Impossible to modify file attributes",
                    "The file cannot be modified", "AddFileAttribute");
            }
        }

        /// <summary>
        ///     - Action : Remove a directory attributes  <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="di">the directory that has to be modified</param>
        /// <param name="attributesToRemove">the attribute to remove</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void RemoveDirAttribute(DirectoryInfo di, FileAttributes attributesToRemove)
        {
            try
            {
                di.Attributes &= ~attributesToRemove;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("the given directory does not exist", "RemoveDirAttribute");
                if (e is UnauthorizedAccessException or SecurityException)
                    throw new AccessException("The given directory cannot be accessed", "RemoveDirAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("the disk is not ready to modify data", "RemoveDirAttribute");
                throw new ManagerException("Writer error", Level.Normal, "Impossible to modify directory attributes",
                    "The path cannot be modified", "RemoveDirAttribute");
            }
        }

        /// <summary>
        ///     - Action : Add an attribute to a directory given with a directoryInfo
        ///     - Implementation : Check
        /// </summary>
        /// <param name="di">the directory that has to be modified</param>
        /// <param name="attribute">the attribute that has to be added</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        private static void AddDirAttribute(DirectoryInfo di, FileAttributes attribute)
        {
            try
            {
                di.Attributes |= attribute;
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException or DirectoryNotFoundException)
                    throw new PathNotFoundException("the given directory does not exist", "AddDirAttribute");
                if (e is UnauthorizedAccessException or SecurityException)
                    throw new AccessException("The given directory cannot be accessed", "AddDirAttribute");
                if (e is IOException)
                    throw new DiskNotReadyException("the disk is not ready to modify data", "AddDirAttribute");
                throw new ManagerException("Writer error", Level.Normal, "Impossible to modify directory attributes",
                    "The path cannot be modified", "AddDirAttribute");
            }
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Set an attribute given in parameter <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <param name="set">whether it has to be set or not</param>
        /// <param name="fa">the attribute that has to be set or not</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void SetAttributes(string path, bool set, FileAttributes fa)
        {
            if (File.Exists(path))
                try
                {
                    if (set)
                        AddFileAttribute(path, fa);
                    else
                        RemoveFileAttribute(new FileInfo(path), fa);
                }
                catch (Exception e)
                {
                    if (e is SecurityException or UnauthorizedAccessException)
                        throw new AccessException("the file given " + path + " access is denied", "SetAttributes");
                    throw new ManagerException();
                }

            if (Directory.Exists(path))
                try
                {
                    var di = new DirectoryInfo(path);
                    if (set)
                        AddDirAttribute(di, fa);
                    else
                        RemoveDirAttribute(di, fa);
                    return;
                }
                catch (SecurityException)
                {
                    throw new AccessException("the directory given " + path + " access is denied", "SetAttributes");
                }

            throw new PathNotFoundException("the given path " + path + " does not exist", "SetAttributes");
        }

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="SetAttributes(string,bool,System.IO.FileAttributes)" />
        /// </summary>
        /// <param name="ft">a fileType associated to a file</param>
        /// <param name="set">whether it has to be set or not</param>
        /// <param name="fa">the attribute that has to be set or not</param>
        /// <exception cref="AccessException">the given path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public static void SetAttributes(LocalPointer ft, bool set, FileAttributes fa) => SetAttributes(ft.Path, set, fa);

        #endregion
    }
}