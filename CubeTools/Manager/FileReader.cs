using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualBasic.FileIO;

namespace CubeTools.FileManager
{
    public class FileReader
    {
        #region Get
        // IsFile : Returns if the given path is a file 
        public bool IsFile(string path)
        {
            return File.Exists(path);
        }

        // IsDirectory : Returns if the given path is a directory
        public bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }

        public string GetParent(string path)
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

        public bool IsFileHidden(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Hidden) != 0);

        }

        public bool IsFileCompressed(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Compressed) != 0);
        }

        public bool IsFileAnArchive(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.Archive) != 0);
        }

        public bool IsASystemFile(string path)
        {
            return (File.Exists(path) && (File.GetAttributes(path) & FileAttributes.System) != 0);
        }

        #endregion

        // In this section, every function is related to the save of file and the creation of FileType
        // instance to simplify the interaction with UI
        #region Saver

        // ReadFile : Get the file using its path and save its information in a FileType
        public FileType ReadFileType(string path)
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
        public void ReadFileType(ref FileType ft)
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
