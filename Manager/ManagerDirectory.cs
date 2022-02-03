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
        // This region contains every variables of the DirectoryType class

        // Attributes
        private string _path;
        private List<FileType> _childrenFiles;
        private long _size;
        private string _date;
        private bool _hide;

        // Getter and Setter
        public string Path { get { return _path; } set { _path = value; } }
        public List<FileType> ChildrenFiles { get { return _childrenFiles; }}


        #endregion

        #region Init
        // This region will generate correctly the DirectoryType with FileTypes

        // Constructors
        public DirectoryType()
        {
            _path = Directory.GetCurrentDirectory();
            _childrenFiles = new List<FileType>();
            foreach (var file in Directory.GetFiles(_path))
                _childrenFiles.Add(GetChild(file));
            _size = ManagerReader.GetFileSize(_path);
            _hide = ManagerReader.IsDirHidden(_path);
            _date = ManagerReader.GetFileCreationDate(_path);
        }

        public DirectoryType(string path) : base() {
            Directory.SetCurrentDirectory(path);
            _path = Directory.GetCurrentDirectory();
            if (_childrenFiles != null)
                _childrenFiles.Clear();
            else
                _childrenFiles = new List<FileType> ();
            foreach (var file in Directory.GetFiles(_path))
                _childrenFiles.Add(GetChild(file));
            foreach (var dir in Directory.GetDirectories(_path))
                _childrenFiles.Add(GetChild(dir));
        }

        #endregion

        #region FileType
        // This region contains every functions that can have access to fileType through directoryType

        /// <summary>
        /// This function creates a FileType and return it using a path
        /// Implementation : NOT Check
        /// </summary>
        public FileType GetChild(string path)
        {
            if (path != null && (File.Exists(path) || Directory.Exists(path)))
            {
                FileType ft = new FileType(path);
                ManagerReader.ReadFileType(ref ft);
                return ft;
            }
            return new FileType();
        }

        /// <summary>
        /// Remove every children in childrenfiles and set them
        /// Implemantion : NOT Check
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
            foreach (var dir in Directory.GetDirectories(_path))
                _childrenFiles.Add(GetChild(dir));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns>if the directoryType has the given child</returns>
        public bool HasChild(string path)
        {
            foreach (FileType ft in _childrenFiles)
            {
                if (ft.Path == path)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <returns></returns>
        private FileSystemEventHandler ActualizeFiles() {
            return null;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Chnage the current directory and remove children files
        /// Implentation : Check
        /// </summary>
        public void ChangeDirectory()
        {
            foreach (var file in _childrenFiles)
                file.Dispose();
            _path = "";
            _size = 0;
        }

        #endregion

        #region CommandLine
        // NO NEED TO BE IMPLEMENTED, DEBUG FUNCTIONS

        public void DisplayChildren()
        {
            Console.WriteLine();
            Console.WriteLine("LastWriteTime               Size                     Name");
            Console.WriteLine("-------------               ----                     ----");
            foreach(var file in _childrenFiles)
            {
                Console.Write(ConstructMessage("LWT", file.LastDate));
                if (file.IsDir)
                    Console.Write("                            ");
                else
                    Console.Write(ConstructMessage("S", file.Size.ToString()));
                Console.WriteLine(ConstructMessage("N", file.Name));
            }
            Console.WriteLine("");
        }

        // Construct message with specific size of string.
        // Type is just a string value 
        private string ConstructMessage(string type, string value)
        {
            string msg = value;
            int max_size = 0;
            switch (type)
            {
                case "LWT":
                    max_size = 28;
                    break;
                case "S":
                    max_size = 28;
                    break;
                case "N":
                    max_size = 32;
                    break;
            }
            for (int i = 0; i < max_size - value.Length; i++)
            {
                msg += " ";
            }
            return msg;
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
