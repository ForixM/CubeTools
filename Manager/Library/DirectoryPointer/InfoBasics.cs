using System.Security;
using Library.ManagerExceptions;

namespace Library.DirectoryPointer
{
    public partial class DirectoryPointer : Pointer
    {
        // This region contains every functions that give information of a pointer (BASICS)

        // This region contains all Get function that also give information of files and directories
        // Reader function useful for little treatment

        /// <summary>
        /// Simply determine whenever the instance exist and is referred to an existing item
        /// </summary>
        public override bool Exist() => Directory.Exists(Path);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action Get the parent directory full name of the file pointer<br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="PathNotFoundException">the file / folder does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override string GetParent()
        {
            try
            {
                return DirectoryInfo is not null
                    ? ""
                    : DirectoryInfo!.FullName;
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException or SecurityException)
                    throw new AccessException(Path + " access denied", "GetParent");
                throw new ManagerException("Access to parent", "High", "Parent not found",
                    "Error while trying to get the Directory of " + Path, "GetParent");
            }
        }

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Get the file size in byte <br></br>
        /// </summary>
        /// <returns>the size of the file or directory</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override long GetPointerSize()
        {
            try
            {
                return DirectoryInfo!.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException or NullReferenceException)
                    throw new AccessException(Path + " cannot be read", "GetFileSize");
                throw new ManagerException("Reader error", "Medium", "Impossible to enumerate files",
                    Path + " and their children could not be read", "GetFileSize");
            }
        }

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Returns the extension of the file pointer<br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>Extension of a pointer</returns>
        public override string GetFileExtension() => "";

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Read the file content <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <returns>the content</returns>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="PathFormatException">the format of the path is incorrect</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override string GetContent()
        {
            if (!Exist()) throw new CorruptedPointerException(Path + " does exist anymore", "GetContent");
            return "";
        }
        
    }
}