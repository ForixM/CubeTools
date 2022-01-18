using System.IO;

namespace Manager
{
    public static class ManagerReader
    {
        #region Properties
        // This region contains every function that give information of the file given with the path
        // Basicaly => bool function

        // IsFile : Returns if the given path is a file 
        // Implementation : Check
        public static bool IsFile(string path)
        {
            return (File.Exists(path)) && (!Directory.Exists(path));
        }

        // IsDirectory : Returns if the given path is a directory
        // Implementation : Check
        public static bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }
        // IsFileHidden : Verify if the file has the property Hidden
        // Implementation : Check
        public static bool IsFileHidden(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Hidden) != 0);

        }
        // IsFileCompressed : Verify if the file has the property Compressed
        // Implementation : Check
        public static bool IsFileCompressed(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Compressed) != 0);
        }
        // IsFileArchived : Verify if the file has the property Archived
        // Implementation : Check
        public static bool IsFileArchived(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Archive) != 0);
        }
        // IsASystemFile : Verify if the file has the property File System
        // Implementation : Check
        public static bool IsASystemFile(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.System) != 0);
        }
        #endregion

        #region Get
        // This region contains all Get function that also give information of
        // files and also directories

        // GetParent : Returns the parent dir using a file or dir contained in it
        
        public static string GetParent(string path)
        {
            if (File.Exists(path))
            {
                return Path.GetDirectoryName(path);
            }
            else
            {
                return "";
            }
        }
        // GetFileCreationDate : Returns the creation date of a file given with the path
        // Implementation : Check
        public static string GetFileCreationDate(string path)
        {
            return File.GetCreationTime(path).ToString();
        }
        // GetFileLastEdition : Returns the last edition of a file
        // Implementation : Check
        public static string GetFileLastEdition(string path)
        {
            return File.GetLastWriteTime(path).ToString();
        }
        // GetFileSize : Returns the size of a file
        // Implementation : Check
        public static string GetFileSize(string path)
        {
            if (File.Exists(path))
            {
                FileInfo fi = new FileInfo(path);
                return fi.Length.ToString();
            }
            return "0";
        }
        #endregion

        #region Saver
        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI

        // ReadFile : Get the file using its path and save its information in a FileType
        public static FileType ReadFileType(string path)
        {
            FileType ft = new FileType(path);
            ft.AbsPath = path;
            if (File.Exists(path))
            {
                FileAttributes fa = File.GetAttributes(ft.AbsPath);
            }

            return null;
        }

        // ReadFileType(ref FileType ft) : modify parameters of ft 
        public static void ReadFileType(ref FileType ft)
        {
            if (File.Exists(ft.AbsPath))
            {
                ft.ReadOnly = ((File.GetAttributes(ft.AbsPath) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
                ft.Hidden = ((File.GetAttributes(ft.AbsPath) & FileAttributes.Hidden) == FileAttributes.Hidden);
            }
            else
            {
                ft.Dispose();
            }
        }
        #endregion

        #region Processing

        #endregion
    }

}
