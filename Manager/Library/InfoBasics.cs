using System.Security;
using Library.ManagerExceptions;

namespace Library
{
    public abstract partial class Pointer
    {
        // This region contains every functions that give information of a pointer (BASICS)

        // This region contains all Get function that also give information of files and directories
        // Reader function useful for little treatment

        /// <summary>
        /// Simply determine whenever the instance exist and is referred to an existing item
        /// </summary>
        public abstract bool Exist();

        /// <summary>
        /// Simply determine whenever the instance has a valid path (Length, character ...)
        /// </summary>
        public bool IsValid() => Exist() && ManagerReader.ManagerReader.IsPathCorrect(Path);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action Get the parent directory full name <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="PathNotFoundException">the file / folder does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public abstract string GetParent();

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : using Path class, it generates the root's path of the instance <br></br>
        ///     - Implementation : CHECK <br></br>
        ///     - No Catching on Format Exception because of the safety ensured by the pointer class
        /// </summary>
        /// <returns>Returns the root's path of the given path</returns>
        public string GetRootPath() => System.IO.Path.GetPathRoot(Path)!;

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Get the file size in byte <br></br>
        ///     - Implementation : NOT CHECK (OPTIMISATION USING THREADS) <br></br>
        ///     - Consider using the Pointer's method instead to increase performance
        /// </summary>
        /// <returns>the size of the file or directory</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="DiskNotReadyException">the disk is refreshing</exception>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public abstract long GetPointerSize();

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Reformat an absolute path to the name of the file or dir <br></br>
        ///     - Implementation : CHECK <br></br>
        ///     - No Catching on Format Exception because of the safety ensured by the pointer class
        /// </summary>
        /// <returns>A string that represents the name of an absolute path</returns>
        public string GetPathToName() => System.IO.Path.GetFileName(Path);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Same as GetPathToName but does not give the extension of the file <br></br>
        ///     - Implementation : CHECK <br></br>
        ///     - No Catching on Format Exception because of the safety ensured by the pointer class
        /// </summary>
        /// <returns>A string that represents the name of the file without its extension</returns>
        public string GetPathToNameNoExtension() => System.IO.Path.GetFileNameWithoutExtension(Path);

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Reformat a name to the absolute path if the given name and current directory are correct <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <returns>The full path of a file or directory</returns>
        /// <exception cref="AccessException">The instance cannot be read</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public string GetNameToPath()
        {
            try
            {
                return System.IO.Path.GetFullPath(Name).Replace("\\", "/");
            }
            catch (Exception e)
            {
                if (e is SecurityException or UnauthorizedAccessException)
                    throw new AccessException(Name + " cannot be read", "GetNameToPath");
                throw new ManagerException("Reader error", "Medium", "Transform absolute path to name",
                    Name + " could not be read", "GetNameToPath");
            }
        }

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Returns the extension of a file. If it is a directory, it will return "" <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>Extension of a pointer</returns>
        public abstract string GetFileExtension();

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Read a file and returns its content <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <returns>the content</returns>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="PathFormatException">the format of the path is incorrect</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public abstract string GetContent();
    }
}