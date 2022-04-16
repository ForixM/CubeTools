using System.IO;
using Library.ManagerExceptions;
using Library.Pointers;

namespace Library.ManagerReader
{
    public static partial class ManagerReader
    {
        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI

        /// <summary>
        ///     - Type : High Level, passed by string and generation of a pointer <br></br>
        ///     - Action : Reads the properties of a File, modifies and inits its associated FileType <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path to save into a fileType</param>
        /// <returns>the file type associated</returns>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="ManagerException">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public static FileType ReadFileType(string path)
        {
            var ft = new FileType(path);
            return ft;
        }

        /// <summary>
        ///     - Type : High Level, pointer passed by reference <br></br>
        ///     - Action : Update FileType passed by reference <br></br>
        ///     - Implementation : Check <br></br>
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
    }
}