using System;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace Manager
{
    public class DirectoryType
    {
        
        #region Variables

        // This region contains every variables of the DirectoryType class

        // Attributes
        private string _path;
        private string _name;
        private List<FileType> _childrenFiles;
        private long _size;
        private string _date;
        private string _lastDate;
        private string _accessDate;
        private bool _hidden;
        private bool _readOnly;

        // Getter and Setter
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public List<FileType> ChildrenFiles => _childrenFiles;
        public long Size => _size;
        public string Date => _date;
        public string LastDate => _lastDate;
        public string AccessDate => _accessDate;
        public bool Hidden => _hidden;
        public bool ReadOnly => _readOnly;

        #endregion

        #region Init

        // This region will generate correctly the DirectoryType with FileTypes

        // Constructors
        public DirectoryType()
        {
            _name = "";
            _path = "";
            _childrenFiles = null;
            _size = 0;
            _date = "";
            _lastDate = "";
            _accessDate = "";
            _hidden = false;
            _readOnly = false;
        }

        public DirectoryType(string path)
        {
            if (Directory.Exists(path))
            {
                try { Directory.SetCurrentDirectory(path); }
                catch (SecurityException) { }
                _path = path;
                _childrenFiles = new List<FileType>();
                foreach (var file in Directory.GetFiles(_path))
                    _childrenFiles.Add(GetChild(file));
                foreach (var dir in Directory.GetDirectories(_path))
                    _childrenFiles.Add(GetChild(dir));
            }

        }

        // Others
        protected bool Equals(DirectoryType other)
        {
            return _path == other._path && _name == other._name && Equals(_childrenFiles, other._childrenFiles) && _size == other._size && _date == other._date && _lastDate == other._lastDate && _accessDate == other._accessDate && _hidden == other._hidden && _readOnly == other._readOnly;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DirectoryType) obj);
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
        /// - Action : Add a file and create it to the directory
        /// </summary>
        /// <param name="name">the name of the file with no extension</param>
        /// <param name="extension">the extension of the file</param>
        public void AddFile(string name, string extension)
        {
            string path = $"{_path}/{name}.{extension}";
            FileType ft = ManagerWriter.Create(path, extension);
            ManagerReader.ReadFileType(ref ft);
            _childrenFiles.Add(ft);
        }

        public void AddDir(string name)
        {
            FileType ft = ManagerWriter.CreateDir(_path + "/" + name);
            ManagerReader.ReadFileType(ref ft);
            _childrenFiles.Add(ft);
        }

        /// <summary>
        /// NOT IMPLEMENTED
        /// </summary>
        /// <returns></returns>
        private FileSystemEventHandler ActualizeFiles()
        {
            return null;
        }

        #endregion

        #region Delete

        /// <summary>
        /// Change the current directory and remove children files
        /// Implentation : NOT Check
        /// NOT PERFECT
        /// </summary>
        public bool ChangeDirectory(string dest)
        {
            if (Directory.Exists(dest))
            {
                Directory.SetCurrentDirectory(dest);
                this.Delete();
                return true;
            }

            return false;
        }

        public void Delete()
        {
            // Delete all files
            foreach (var ft in _childrenFiles)
            {
                ManagerWriter.Delete(ft);
            }
            _childrenFiles = new List<FileType>();
            // Delete directory
            ManagerWriter.DeleteDir(_path);
            _path = "";
            _size = 0;
            Dispose();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Operator

        private bool IsNull() => !Directory.Exists(_path);
        public static bool operator==(DirectoryType dir1, DirectoryType dir2)
        {
            return Equals(dir1, dir2);
        }
        public static bool operator!=(DirectoryType dir1, DirectoryType dir2)
        {
            return !Equals(dir1, dir2);
        }
        
        #endregion
        
        #region CommandLine

        // NO NEED TO BE IMPLEMENTED, DEBUG FUNCTIONS

        public void DisplayChildren()
        {
            Console.WriteLine();
            Console.WriteLine("LastWriteTime               Size                     Name");
            Console.WriteLine("-------------               ----                     ----");
            foreach (var file in _childrenFiles)
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
            int maxSize = 0;
            switch (type)
            {
                case "LWT":
                    maxSize = 28;
                    break;
                case "S":
                    maxSize = 28;
                    break;
                case "N":
                    maxSize = 32;
                    break;
            }

            for (int i = 0; i < maxSize - value.Length; i++)
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