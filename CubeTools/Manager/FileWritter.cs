using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace CubeTools.FileManager
{
    public static class FileWritter
    {
        #region Action
        
        // Basic Functions
        // RENAME FUNCTIONS

        // Rename(string path) : Rename with no new path => create a copy with the same name
        public static void Rename(string path)
        {
            if (File.Exists(path))
            {
                int i = 1;
                while (File.Exists(path + "("+i+")"))
                {
                    i += 1;
                }
                File.Move(path, path+"("+i+")");
            }
        }
        // Rename(string path) : Rename with a new path
        public static void Rename(string path, string newPath)
        {
            if (File.Exists(path))
            {
                if (newPath != null)
                {
                    System.IO.File.Move(path, newPath);
                }
            }
        }

        // COPY FUNCTIONS
        // Copy(string path) : using path
        public static void Copy(string path)
        {
            if (File.Exists(path))
            {
                File.Copy(path, path);
            }
        }
        // CREATE FUNCTIONS
        // Create(string path)
        public static FileType Create(string path)
        {
            File.Create(path);
            if (File.Exists(path))
            {
                FileType ft = new FileType(path);
            }
            else
            {
                return null;
            }

            return null;
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
            if (FileType.type.Contains(newType))
            {
                file.IconUI = null; // Modified here the new icon to put
            }
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
