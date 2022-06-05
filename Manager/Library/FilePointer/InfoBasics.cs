using System.Security;
using Library.ManagerExceptions;

namespace Library.FilePointer
{
    public partial class FilePointer : Pointer
    {
        // This region contains every functions that give information of a pointer (BASICS)

        // This region contains all Get function that also give information of files and directories
        // Reader function useful for little treatment

        /// <summary>
        /// Simply determine whenever the instance exist and is referred to an existing item
        /// </summary>
        public override bool Exist() => !IsDir && File.Exists(_path);
        

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action Get the parent directory full name <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>the parent string name</returns>
        /// <exception cref="AccessException">the path cannot be accessed</exception>
        /// <exception cref="PathNotFoundException">the file / folder does not exist</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override string GetParent()
        {
            try
            {
                return FileInfo!.DirectoryName!;
            }
            catch (Exception e)
            {
                if (e is UnauthorizedAccessException or SecurityException or NullReferenceException)
                    throw new AccessException(Path + " access denied", "GetParent");
                throw new ManagerException("Access to parent", Level.High, "Parent not found",
                    "Error while trying to get the Directory of " + Path, "GetParent");
            }
        }

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
        public override long GetPointerSize()
        {
            try
            {
                return FileInfo!.Length;
            }
            catch (Exception e)
            {
                throw e switch
                {
                    SecurityException or UnauthorizedAccessException or NullReferenceException =>
                        new AccessException(Path + " cannot be read", "GetFileSize"),
                    IOException => new DiskNotReadyException(Path + " cannot be read", "GetFileSize"),
                    _ => new ManagerException("Reader error", Level.Normal, "Unable to enumerate files",
                        Path + " and their children could not be read", "GetFileSize")
                };
            }
        }
        

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Returns the extension of a file. If it is a directory, it will return "" <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <returns>Extension of a pointer</returns>
        public override string GetFileExtension()
        {
            string extension = System.IO.Path.GetExtension(Path);
            return extension.Length == 0 ? "" : extension.Remove(0, 1);
        }

        /// <summary>
        ///     - Type : Low Level <br></br>
        ///     - Action : Read a file and returns its content <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <returns>the content</returns>
        /// <exception cref="PathNotFoundException">the path does not exist</exception>
        /// <exception cref="PathFormatException">the format of the path is incorrect</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public override string GetContent()
        {
            if (!Exist()) throw new CorruptedPointerException(Path + " does exist anymore", "GetContent");
            try
            {
                var sr = new StreamReader(Path);
                var res = sr.ReadToEnd();
                sr.Close();
                return res;
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case IOException:
                        throw new PathFormatException(Path + " format is incorrect", "GetContent");
                    case OutOfMemoryException:
                        Console.Error.WriteLine(Path + " is too large to be read");
                        return "";
                    default:
                        throw new ManagerException("Reader Error", Level.Normal, "Out Of Memory",
                            "The content of " + Path + " is unreadable, a memory error occured", "GetContent");
                }
            }
        }
    }
}