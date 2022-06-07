using Library.ManagerExceptions;
using Microsoft.VisualBasic.FileIO;

namespace Library.ManagerWriter
{
    public static partial class ManagerWriter
    {
        // DELETE FUNCTIONS

        /// <summary>
        ///     - Low Level => Delete a file using its path if it exists <br></br>
        ///     - Action : Delete a file <br></br>
        ///     - Implementation : NOT CHECK
        /// </summary>
        /// <param name="path">the path of the file</param>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void DeleteFile(string path)
        {
            if (!File.Exists(path))
                throw new PathNotFoundException(path + " does not exist", "Delete");
            try
            {
                FileSystem.DeleteFile(path, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin, UICancelOption.ThrowException);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new InUseException(path + " is used by another process", "Delete"),
                    UnauthorizedAccessException => new AccessException(path + " access is denied", "Delete"),
                    _ => new ManagerException("Delete is impossible", Level.Normal, "Writer Error",
                        path + " could not be deleted", "Delete")
                };
            }
        }

        /// <summary>
        ///     - High Level => Delete a file using its associated FilePointer <br></br>
        ///     - Action : Delete a file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="fileLocalPointer">the file pointer that is associated to a file</param>
        /// <exception cref="InUseException">The given path is already used</exception>
        /// <exception cref="AccessException">The given path could not be accessed</exception>
        /// <exception cref="ManagerException">An Error occurred</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void DeleteFile(FilePointer.FileLocalPointer fileLocalPointer)
        {
            if (!fileLocalPointer.Exist()) throw new CorruptedPointerException(fileLocalPointer.Path + " does not exist anymore", "Delete");
            DeleteFile(fileLocalPointer.Path);
            fileLocalPointer.Dispose();
        }

        /// <summary>
        ///     - Low Level : Delete a directory using its path <br></br>
        ///     - Action : Delete a directory using its path, recursive variable indicate if all subdirectories has to be deleted
        ///     <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the directory path</param>
        /// <param name="recursive">whether all content inside the directory should be deleted</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void DeleteDir(string path, bool recursive = true)
        {
            if (!Directory.Exists(path))
                throw new PathNotFoundException(path + " does not exist", "DeleteDir");
            try
            {
                FileSystem.DeleteDirectory(path,
                    recursive
                        ? DeleteDirectoryOption.DeleteAllContents
                        : DeleteDirectoryOption.ThrowIfDirectoryNonEmpty);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new SystemErrorException("directory not empty or system blocked " + path,
                        "DeleteDir"),
                    UnauthorizedAccessException => new AccessException(path + " access denied", "DeleteDir"),
                    _ => new ManagerException("Impossible to Delete", Level.Normal, "Writer Error",
                        path + " could not be deleted", "DeleteDir")
                };
            }
        }

        /// <summary>
        ///     - High Level : Delete a directory using its associated class <br></br>
        ///     - Action : Delete a directory using its class, recursive variable indicate if all subdirectories has to be deleted <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="directoryLocalPointer">the directory pointer</param>
        /// <param name="recursive">whether all content inside the directory should be deleted</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void DeleteDir(DirectoryPointer.DirectoryLocalPointer directoryLocalPointer, bool recursive = true)
        {
            // Load the directoryPointer and delete content
            
            // Depending on the type of folder, choose the best alternative
            // Loaded contains sub-folders, recall recursive function
            var list = directoryLocalPointer.GetChildren();
            if (list.Any(pointer => pointer is DirectoryPointer.DirectoryLocalPointer)) 
                DeleteDir(list, recursive);
            else DeleteDir(directoryLocalPointer.Path, recursive);
            
            // Dispose the pointer
            directoryLocalPointer.Dispose();
            GC.Collect();
        }

        /// <summary>
        ///     - High Level : Delete directories <br></br>
        ///     - Action : Delete a directories using their classes, recursive variable indicate if all subdirectories / files have
        ///     to be deleted <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="ftList">the fileType list</param>
        /// <param name="recursive">if it has to suppress all sub-directories</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">One of the given pointers does not exist</exception>
        public static void DeleteDir(List<LocalPointer> ftList, bool recursive = true)
        {
            foreach (var pointer in ftList)
            {
                switch (pointer)
                {
                    case FilePointer.FileLocalPointer filePointer:
                        DeleteFile(filePointer);
                        break;
                    case DirectoryPointer.DirectoryLocalPointer directoryPointer:
                        DeleteDir(directoryPointer, recursive);
                        break;
                }
            }
        }

        /// <summary>
        ///     - Type : High Level  <br></br>
        ///     - Action : Delete a directory or a file <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="source">the data source</param>
        /// <param name="recursive">whether all content inside the directory should be deleted</param>
        /// <exception cref="SystemErrorException">the system blocked app</exception>
        /// <exception cref="AccessException">Access has been denied</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public static void Delete(string source, bool recursive = false)
        {
            if (File.Exists(source)) DeleteFile(source);
            else DeleteDir(source, recursive);
        }
    }
}