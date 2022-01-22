using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class DirectoryType
    {
        #region Variables

        // Attributes
        private string _path;
        public List<FileType> _childrenFiles;
        private int _size;
        private string _date;
        private bool _hide;

        // Getter and Setter
        public string Path { get { return _path; } set { _path = value; } }
        public List<FileType> ChildrenFiles {get {return _childrenFiles;}}


        #endregion

        #region Init

        // Constructors
        public DirectoryType()
        {
            _path = Directory.GetCurrentDirectory();
            _childrenFiles = new List<FileType>();
            foreach (var file in Directory.GetFiles(_path))
                _childrenFiles.Add(GetChild(file));
            _size = 0;
            _hide = false;
            _date = "";
        }

        public DirectoryType(string path) : base() => _path = path;

        #endregion

        #region FileType

        /// <summary>
        /// This function creates a FileType and return it using a path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public FileType GetChild(string path)
        {
            if (path == null || !File.Exists(path) || !Directory.Exists(path))
            {
                FileType ft = new();
                return ft;
            }
            else
            {
                FileType fileType = new FileType(path);
                ManagerReader.ReadFileType(ref fileType);
                return fileType;
            }
        }

        /// <summary>
        /// Remove every children in childrenfiles and set them
        /// </summary>
        public void SetChildrenFiles()
        {
            foreach (var file in _childrenFiles)
            {
                file.Dispose();
            }
            _childrenFiles.Clear();
            foreach (var file in Directory.GetFiles(Path))
                _childrenFiles.Add(GetChild(file));
            
        }

        #endregion

        #region CommandLine
        
        public void DisplayChildren()
        {
            Console.WriteLine("LastWriteTime              Size      Name");
            foreach(var file in _childrenFiles)
            {
                Console.Write(file.LastDate + "    ");
                if (file.IsDir)
                    Console.Write("    ");
                else
                    Console.Write(file.Size + "    ");
                Console.WriteLine(ManagerReader.GetPathToName(file.Path));
            }
            Console.WriteLine("");
        }

        #endregion

        #region Delete

        public void ChangeDirectory()
        {
            foreach (var file in _childrenFiles)
                file.Dispose();
            _path = "";
            _size = 0;
        }

        #endregion

        #region Debug

        public void PrintInformation()
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("Directory : " + _path);
            Console.WriteLine("----------------------");
            foreach (var child in _childrenFiles)
                child.PrintInformation();
        }
        #endregion
    }
}
