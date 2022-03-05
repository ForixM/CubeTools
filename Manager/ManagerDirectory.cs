using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security;
using Manager.ManagerExceptions;

namespace Manager
{
    /// <summary>
    /// This class represents a pointer to a loaded directory
    /// It will managed our file tree and will use actions to modify files efficiently
    /// </summary>
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

        public List<FileType> ChildrenFiles {
            get { return _childrenFiles; }
            set { _childrenFiles = value; }
        }
        public long Size => _size;
        public string Date => _date;
        public string LastDate => _lastDate;
        public string AccessDate => _accessDate;
        public bool Hidden => _hidden;
        public bool ReadOnly => _readOnly;

        #endregion

        #region Init

        // This region will generate correctly the DirectoryType with FileTypes
        //
        /// <summary>
        /// - Action : Default constructor of DirectoryType class
        /// - Implementation : Check
        /// </summary>
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

        /// <summary>
        /// - Action : Load a pointer to a directory given in the parameter with its path
        /// - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the directory</param>
        /// <exception cref="AccessException">the given directory cannot be accessed</exception>
        /// <exception cref="SystemErrorException"></exception>
        /// <exception cref="ManagerException"></exception>
        public DirectoryType(string path)
        {
            // Verify if the directory already exists and its path is correct
            path = path.Replace('\\', '/');
            if (Directory.Exists(path) && !String.IsNullOrEmpty(path))
            {
                try
                {
                    Directory.SetCurrentDirectory(path); // IOException, SecurityException, 
                }
                catch (Exception e)
                {
                    if (e is IOException)
                        throw new SystemErrorException(path + " has been blocked by system", "Directory Constructor");
                    if (e is SecurityException)
                        throw new AccessException(path + " could be not accessed", "Directory Constructor");
                    throw new ManagerException("ManagerException", "High", "GenerateDirectory",
                        "Generate directory was impossible", "Directory Constructor");
                }

                try
                {
                    _path = path;
                    _name = ManagerReader.GetPathToName(path);
                    _date = ManagerReader.GetFileCreationDate(path);
                    _lastDate = ManagerReader.GetFileLastEdition(path);
                    _accessDate = ManagerReader.GetFileAccessDate(path);
                    _size = ManagerReader.GetFileSize(path);
                    _hidden = ManagerReader.IsFileHidden(path);
                    _readOnly = ManagerReader.IsReadOnly(path);
                    _childrenFiles = new List<FileType>();
                    SetChildrenFiles();
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Directory could not be created");
                }
            }
            else 
                throw new PathNotFoundException(path + " could not be identified","Directory Constructor");
        }

        #endregion

        #region FileType

        // This region contains every functions that can have access to fileType through directoryType

        /// <summary>
        /// This function creates a FileType and return it using a path
        /// Implementation : NOT Check
        /// </summary>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="Exception">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
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
        /// - Action : Remove every children in ChildrenFiles and set them
        /// - Implementation : NOT Check
        /// </summary>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="Exception">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public void SetChildrenFiles()
        {
            foreach (var file in _childrenFiles)
            {
                file.Dispose();
            }
            _childrenFiles.Clear();
            foreach (var file in Directory.GetFiles(_path))
            {
                _childrenFiles.Add(GetChild(file.Replace('\\','/')));
            }
            foreach (var dir in Directory.GetDirectories(_path))
                _childrenFiles.Add(GetChild(dir.Replace('\\', '/')));
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
            ManagerWriter.Create(path, extension);
            FileType ft = ManagerReader.ReadFileType(path);
            ManagerReader.ReadFileType(ref ft);
            _childrenFiles.Add(ft);
        }

        public void AddDir(string name)
        {
            string path = _path + '/' + name;
            ManagerWriter.CreateDir(path);
            FileType ft = ManagerReader.ReadFileType(path);
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
        /// -Action : Change the current directory and remove children files <br></br>
        /// Implementation : NOT Check <br></br>
        /// NOT PERFECT
        /// </summary>
        public void ChangeDirectory(string dest)
        {
            dest = dest.Replace('\\', '/');
            if (Directory.Exists(dest))
            {
                string newPath = ManagerReader.GetNameToPath(dest);
                // Erase last directory
                foreach (var ft in ChildrenFiles)
                {
                    ft.Dispose();
                }

                ChildrenFiles.Clear();
                // Replace
                try
                {
                    Directory.SetCurrentDirectory(newPath);
                    _path = newPath;
                    SetChildrenFiles();
                }
                catch (IOException)
                {
                    throw new SystemErrorException("system blocked " + newPath, "ChangeDirectory");
                }
                catch (SecurityException)
                {
                    throw new AccessException(newPath + " could not be accessed", "ChangeDirectory");
                }
            }

            throw new PathNotFoundException(dest + " does not exist", "ChangeDirectory");
        }

        /// <summary>
        /// Delete function : delete the directory, its associated files in a high context level
        /// Implementation : Not Check
        /// </summary>
        public void Delete()
        {
            // Delete all files
            foreach (var ft in _childrenFiles)
            {
                ManagerWriter.Delete(ft);
            }
            _childrenFiles = new List<FileType>();
            // Delete directory
            ManagerWriter.DeleteDir(_path, false);
            _path = "";
            _size = 0;
            Dispose();
        }

        /// <summary>
        /// Dispose the directoryType to 
        /// </summary>
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
        
        // Others
        protected bool Equals(DirectoryType other)
        {
            return _path == other._path && _name == other._name && Equals(_childrenFiles, other._childrenFiles) 
                   && _size == other._size && _date == other._date && _lastDate == other._lastDate 
                   && _accessDate == other._accessDate && _hidden == other._hidden && _readOnly == other._readOnly;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DirectoryType) obj);
        }

        #endregion
        
        #region CommandLine

        // NO NEED TO BE IMPLEMENTED, DEBUG FUNCTIONS

        /// <summary>
        /// Display all children of the current directory pointer
        /// </summary>
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