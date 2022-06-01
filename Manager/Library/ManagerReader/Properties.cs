using System.Security;
using Library.ManagerExceptions;

namespace Library.ManagerReader
{
    public static partial class ManagerReader
    {
        // This region contains every function that give information of the file given with the path or pointer

        /// <summary>
        ///     - Type : Low Level  <br></br>
        ///     - Action : Verify whether a file or a directory is hidden or not  <br></br>
        ///     - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>Whether it is hidden or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileHidden(string path) => HasAttribute(FileAttributes.Hidden, path);

        /// <summary>
        ///     - Type : High Level  <br></br>
        ///     -> <see cref="IsFileHidden(string)" />
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>Whether it is hidden or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileHidden(Pointer ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsFileHidden");
            return IsFileHidden(ft.Path);
        }


        /// <summary>
        ///     _ Type : Low Level  <br></br>
        ///     - Action : Verify whether a file or directory is compressed  <br></br>
        ///     - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>Whether it is compressed or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileCompressed(string path) => HasAttribute(FileAttributes.Compressed, path);

        /// <summary>
        ///     _ Type : High Level  <br></br>
        ///     -> <see cref="IsFileCompressed(string)" />
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>Whether it is compressed or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileCompressed(Pointer ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsFileCompressed");
            return IsFileCompressed(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level  <br></br>
        ///     - Action : Verify whether a file or a directory is archived  <br></br>
        ///     - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>Whether it is archived or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileArchived(string path) => HasAttribute(FileAttributes.Archive, path);

        /// <summary>
        ///     - Type : High Level <br></br>
        ///     -> <see cref="IsFileArchived(string)" />
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>Whether it is archived or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsFileArchived(Pointer ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsFileArchived");
            return IsFileArchived(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level  <br></br>
        ///     - Action : Verify whether a file or directory is part of a the system  <br></br>
        ///     - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>If the file/folder is part of system or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsASystemFile(string path) => HasAttribute(FileAttributes.System, path);

        /// <summary>
        ///     - Type : High Level  <br></br>
        ///     -> <see cref="IsASystemFile(string)" />
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>If the file/folder is part of system or not</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsASystemFile(Pointer ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsASystemFile");
            return IsASystemFile(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level  <br></br>
        ///     - Action : Verify whether a file or directory is in readOnly  <br></br>
        ///     - Implementation : Check  <br></br>
        /// </summary>
        /// <param name="path">the given file or dir</param>
        /// <returns>If the file is in readOnly</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsReadOnly(string path) => HasAttribute(FileAttributes.ReadOnly, path);

        /// <summary>
        ///     - Type : High Level  <br></br>
        ///     -> <see cref="IsReadOnly(string)" />
        /// </summary>
        /// <param name="ft">the given file or dir (pointer)</param>
        /// <returns>If the file is in readOnly</returns>
        /// <exception cref="InUseException">The given cannot be read because a program is using it</exception>
        /// <exception cref="AccessException">The given path cannot be read because application does not have rights</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="CorruptedPointerException">The pointer is corrupted</exception>
        /// <exception cref="ManagerException">Error could not be identified</exception>
        public static bool IsReadOnly(Pointer ft)
        {
            if (!File.Exists(ft.Path) && !Directory.Exists(ft.Path))
                throw new CorruptedPointerException("pointer of file " + ft.Path + " should be destroyed",
                    "IsReadOnly");
            return IsReadOnly(ft.Path);
        }

        /// <summary>
        ///     - Type : Low Level  <br></br>
        ///     - Action : Verify whether the given file or directory has fa attribute <br></br>
        ///     - Implementation : Check  <br></br>
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
                    throw new ManagerException("Reader error", "High", "Impossible to read",
                        "the given path " + path + " could not be read", "HasAttribute");
                }

            // If it is a directory
            if (Directory.Exists(path))
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

            throw new PathNotFoundException(path + " does not exist", "HasAttribute");
        }
    }
}