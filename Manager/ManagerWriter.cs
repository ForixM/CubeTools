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
        /// Implementation : Check
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
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newPath"></param>
        public static void Rename(string path, string newPath)
        {
            if (newPath != null)
            {
                if (File.Exists(path))
                {
                    if (File.Exists(newPath))
                    {
                        int i = 1;
                        while (File.Exists(path + "(" + i + ")"))
                        {
                            i += 1;
                        }
                        File.Move(path, path + "(" + i + ")");
                    }
                    else
                        File.Move(path, newPath);
                }
            }
        }

        // COPY FUNCTIONS

        /// <summary>
        /// Overload 1 : Copy function without copy file name <br></br>
        /// Copy the content of the file path and create a copy of it using path(${i}) format
        /// Implementation : Check
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
        /// Implementation : Check
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

        // CREATE/DELETE FUNCTIONS

        /// <summary>
        /// Create a file
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        public static void Create(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
                File.Create(path);
        }

        /// <summary>
        /// Create a file with an extension
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        public static void Create(string path, string type)
        {
            File.Create(path+"."+type);
        }

        /// <summary>
        /// Create a dir and if it already exists, modify the document name
        /// Implementation : Check
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDir(string path)
        {
            if (File.Exists(path))
            {
                int i = 1;
                while (Directory.Exists($"{path}({i})")) ;
                {
                    i += 1;
                }
                Directory.CreateDirectory($"{path}({i})");
            }
            else
            {
                Directory.CreateDirectory(path);
            }
        }

        // Implementation Check
        public static void Delete(string path) { File.Delete(path); }
        // Implementation Check
        public static void DeleteDir(string path)
        {
            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

        #endregion
    
    }
}
