using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Manager
{
    public static class ManagerWriter
    {
        #region Properties
        // This region contains every function that set information of the file given with the path
        // Basicaly => setters of properties

        // SetProperties functions : Basicaly set the property for the file given in argument

        // Implementation : Check
        public static void SetFileHidden(string path) { File.SetAttributes(path, FileAttributes.Hidden); }
        // Implementation : Not check
        public static void SetFileCompressed(string path) { File.SetAttributes(path, FileAttributes.Compressed); }
        // Implementation : Check
        public static void SetFileSystem(string path) { File.SetAttributes(path, FileAttributes.System); }
        // Implementation : Check
        public static void SetFileArchived(string path) { File.SetAttributes(path, FileAttributes.Archive); }

        #endregion

        #region Action

        // RENAME FUNCTIONS

        /// <summary>
        /// Overload 1 : Rename with no path dest <br></br>
        /// Rename a file without specified name and does not overwrite the file if there is one which has the same path <br></br>
        /// Instead of deleting the file, the function add a number ${i} as a name
        /// </summary>
        /// <param name="path"></param>
        public static void Rename(string path)
        {
            if (File.Exists(path))
            {
                int i = 1;
                while (File.Exists(path + "(" + i + ")"))
                {
                    i += 1;
                }
                File.Move(path, path + "(" + i + ")");
            }
        }
        /// <summary>
        /// Overload 2 : Rename with a path dest<br></br>
        /// Rename a file with newPath and does not overwrite the file if there is one which has the same path <br></br>
        /// Instead of deleting the file, the function add a number ${i} as a name <br></br>
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        public static void Rename(string path, string newPath)
        {
            if (newPath != null)
            {
                if (File.Exists(path))
                {
                    int i = 1;
                    while (File.Exists(path + "(" + i + ")"))
                    {
                        i += 1;
                    }
                    File.Move(path, path + "(" + i + ")");
                }
                else
                {
                    File.Move(path, newPath);
                }
            }
        }

        // COPY FUNCTIONS

        /// <summary>
        /// Overload 1 : Copy function without copy file name <br></br>
        /// Copy the content of the file path and create a copy of it using path(${i}) format
        /// </summary>
        /// <param name="path">The source path</param>
        public static void Copy(string path)
        {
            if (File.Exists(path))
            {
                int i = 1;
                while (File.Exists($"{path}({i})"));
                {
                    i += 1;
                }
                File.Copy(path, $"{path}({i})");
            }
        }
        
        /// <summary>
        /// Overload 2 : Copy the content in the source file into the dest file
        /// If the dest file already exists, then rename the copy
        /// </summary>
        /// <param name="source">The source file</param>
        /// <param name="dest">The dest file</param>
        public static void Copy(string source, string dest)
        {
            if (File.Exists(source))
            {
                if (File.Exists(dest))
                    Copy(dest);
                else
                    File.Copy(source, dest);
            } 
        }

        /// <summary>
        /// Overload 3 : Copy the content and choose to overwrite or not
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="overwrite"></param>
        public static void Copy(string source, string dest, bool overwrite)
        {
            if (File.Exists(source))
            {
                if (!overwrite && File.Exists(dest))
                    File.Delete(dest);
                Copy(source, dest);
            }
        }

        // CREATE FUNCTIONS

        // Create(string path)
        

        public static void Create(string path)
        {
            File.CreateText(path);
        }

        public static void Create(string path, string type)
        {

        }

        public static void Delete(string abs_path)
        {
            if (File.Exists(abs_path))
            {
                File.Delete(abs_path);
            }
        }


        // ChangeExtension : Modify the extension a file and verify if the given 
        public static void ChangeExtension(string newType, FileType file)
        {
            file.AbsPath = Path.ChangeExtension(file.AbsPath, newType);
            file.Path = Path.GetFileName(file.AbsPath);
            file.Type = newType;
        }

        public static void ChangeReadOnly(bool readonly_)
        {

        }
        #endregion

        public static void SaveFileType(FileType file)
        {
            if (File.Exists(file.Path))
            {
                // Insert code to automatically save the file
            }
        }
    }
}
