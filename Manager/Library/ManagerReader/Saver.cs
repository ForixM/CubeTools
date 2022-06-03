using Library.ManagerExceptions;

namespace Library.ManagerReader
{
    public static partial class ManagerReader
    {
        // In this section, every function is related to the save of file and the creation of FilePointer
        // instance to simplify the interaction with UI

        public static void ReadPointer(ref Pointer pointer)
        {
            switch (pointer)
            {
                case FilePointer.FilePointer filePointer:
                    ReadFilePointer(ref filePointer);
                    break;
                case DirectoryPointer.DirectoryPointer directoryPointer:
                    ReadDirectoryPointer(ref directoryPointer);
                    break;
                default:
                    throw new CorruptedPointerException("invalid pointer on the path " + pointer.Path, "ReadPointer");
            }
        }

        /// <summary>
        ///     - Type : High Level, pointer passed by reference <br></br>
        ///     - Action : Update FilePointer passed by reference <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <param name="pointer">the file pointer to update</param>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        /// <exception cref="PathNotFoundException">the data could not be found on the device</exception>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupted</exception>
        /// <exception cref="ManagerException">an unknown exception occured</exception>
        public static void ReadFilePointer(ref FilePointer.FilePointer pointer)
        {
            pointer.FileInfo = new FileInfo(pointer.Path);
            pointer.Type = GetFileExtension(pointer);
            pointer.Size = GetFileSize(pointer);
            pointer.Name = GetPathToName(pointer);
        }
        
        /// <summary>
        ///     - Type : High Level, pointer passed by reference <br></br>
        ///     - Action : Update FilePointer passed by reference <br></br>
        ///     - Implementation : Check <br></br>
        /// </summary>
        /// <param name="pointer">the directory pointer to update</param>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        /// <exception cref="PathNotFoundException">the data could not be found on the device</exception>
        /// <exception cref="CorruptedPointerException">the given pointer is corrupted</exception>
        /// <exception cref="ManagerException">an unknown exception occured</exception>
        public static void ReadDirectoryPointer(ref DirectoryPointer.DirectoryPointer pointer)
        {
            pointer.DirectoryInfo = new DirectoryInfo(pointer.Path);
            pointer.Type = GetFileExtension(pointer);
            pointer.Size = GetFileSize(pointer);
            pointer.Name = GetPathToName(pointer);
        }
    }
}