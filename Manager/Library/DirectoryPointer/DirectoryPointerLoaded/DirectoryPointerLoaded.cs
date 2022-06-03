using System.Security;
// Project's Library
using Library.ManagerExceptions;

namespace Library.DirectoryPointer.DirectoryPointerLoaded
{
    /// <summary>
    ///     This class represents a pointer to a loaded directory
    ///     It will managed our file tree and will use actions to modify files efficiently
    /// </summary>
    public partial class DirectoryPointerLoaded : DirectoryPointer
    {
        #region Variables

        private List<Pointer> _childrenFiles;

        public List<Pointer> ChildrenFiles { get => _childrenFiles; set => _childrenFiles = value; }

        //protected FileSystemWatcher? _watcher;
        
        // Watcher for raising events 
        //public FileSystemWatcher? Watcher => _watcher;
        
        #endregion

        #region Init

        // This region will generate correctly the DirectoryPointer with FileTypes
        
        
        /// <summary>
        ///     - Action : Default constructor of DirectoryPointer class
        ///     - Implementation : Check
        /// </summary>
        public DirectoryPointerLoaded() : base()
        {
            _childrenFiles = new List<Pointer>();
            //_watcher = null;
        }

        /// <summary>
        ///     - Action : Load a pointer to a directory given in the parameter with its path
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the directory</param>
        /// <exception cref="AccessException">the given directory cannot be accessed</exception>
        /// <exception cref="SystemErrorException">The system blocked the constructor</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public DirectoryPointerLoaded(string path) : base(path)
        {
            try
            {
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception e)
            {
                throw e switch
                {
                    IOException => new SystemErrorException(path + " has been blocked by system", "Directory Constructor"),
                    SecurityException => new AccessException(path + " could be not accessed", "Directory Constructor"),
                    _ => new ManagerException("ManagerException", "High", "GenerateDirectory",
                        "Generate directory was impossible", "Directory Constructor")
                };
            }
            
            _childrenFiles = new List<Pointer>();
            SetChildrenFiles();

            
            // Watcher
            //_watcher = new FileSystemWatcher(path);
            //_watcher.Changed += OnChanged;
            //_watcher.Created += OnCreated;
            //_watcher.Deleted += OnDeleted;
            //_watcher.Renamed += OnRenamed;
            //_watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName
              //                      | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            //_watcher.Filter = "*";
            //_watcher.IncludeSubdirectories = false;
            //_watcher.EnableRaisingEvents = true;
        }

        #endregion
        
        #region Children

        // This region contains every functions that can have access to children through directoryTypeLoaded

        /// <summary>
        ///     This function creates a FilePointer and return it using a path <br></br>
        ///     Implementation : NOT Check
        /// </summary>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="Exception">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public Pointer GetChild(string path)
        {
            var res = ChildrenFiles.FindAll(ft => ft.Path == path);
            return res.Count == 0 ? NullPointer : res[0];
        }

        /// <summary>
        ///     - Action : Remove every children in ChildrenFiles and set them <br></br>
        ///     - Implementation : NOT Check
        /// </summary>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="Exception">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public void SetChildrenFiles()
        {
            // Clear last pointers
            foreach (var pointer in ChildrenFiles) pointer.Dispose();
            ChildrenFiles.Clear();
            
            // Check assignment of DirectoryInfo
            try
            {
                if (_directoryInfo!.FullName.Replace("\\","/") != _path)
                    _directoryInfo = new DirectoryInfo(_path);
            }
            catch (Exception)
            {
                return;
            }

            // Add all sub directories
            foreach (var dir in _directoryInfo!.EnumerateDirectories())
            {
                try
                {
                    ChildrenFiles.Add(new DirectoryPointer(dir.FullName.Replace('\\', '/')));
                }
                catch (Exception) {}
            }

            // Add all sub files
            foreach (var file in _directoryInfo.EnumerateFiles())
            {
                try
                {
                    ChildrenFiles.Add(new FilePointer.FilePointer(file.FullName.Replace('\\', '/')));
                }
                catch (Exception) {}
            }
        }

        /// <summary>
        ///     - Action : Verify if the file or folder given with its path is a child of the directory
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">The given path to verify</param>
        /// <returns>if the directoryType has the given child</returns>
        public bool HasChild(string path) => ChildrenFiles.Any(ft => ft.Path == path);

        /// <summary>
        /// Add single child to ChildrenFiles
        /// </summary>
        /// <param name="pointer">The pointer to add</param>
        public void AddChild(Pointer pointer)
        {
            if (!HasChild(pointer.Path)) ChildrenFiles.Add(pointer);
        }
        
        /// <summary>
        /// Overload of <see cref="AddChild(Pointer)"/>
        /// </summary>
        /// <param name="path">Absolute Path</param>
        /// <param name="isdir">Whether it is a directory or not</param>
        public void AddChild(string path, bool isdir = false)
        {
            if (HasChild(path)) return;
            if (isdir) AddDir(ManagerReader.ManagerReader.GetPathToName(path));
            else AddFile(ManagerReader.ManagerReader.GetPathToName(path), ManagerReader.ManagerReader.GetFileExtension(path));
        }

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Add a file and create it to the directory <br></br>
        ///     - Implementation : CHECK
        /// </summary>
        /// <param name="name">the name of the file with no extension</param>
        /// <param name="extension">the extension of the file</param>
        public void AddFile(string name, string extension)
        {
            var path = $"{Path}/{name}.{extension}";
            if (!HasChild(path)) ChildrenFiles.Add(ManagerWriter.ManagerWriter.Create(path, extension));
        }

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Create a directory and add it to the current loaded directory <br></br>
        ///     - Implementation : CHECK <br></br>
        /// </summary>
        /// <param name="name">The wanted name of the folder</param>
        public void AddDir(string name)
        {
            var path = Path + '/' + name;
            if (!HasChild(path)) ChildrenFiles.Add(ManagerWriter.ManagerWriter.CreateDir(path));
        }

        /// <summary>
        ///     - Type : High Level Method <br></br>
        ///     - Action : Try to remove a file given with a path in the list of _childrenFiles
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the given path</param>
        public void Remove(string path)
        {
            for (var i = 0; i < ChildrenFiles.Count; i++)
            {
                if (ChildrenFiles[i].Path == path)
                {
                    ChildrenFiles[i].Dispose();
                    ChildrenFiles.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        ///     - Action : Remove a file given with a path in the list of _childrenFiles
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="pointer">the given pointer</param>
        public void Remove(Pointer pointer) => Remove(pointer.Path);

        /// <summary>
        ///     - Action : Reload One single pointer given in path. <br></br>
        ///     -> if path is not contained, nothing is done <br></br>
        ///     -> Otherwise, the pointer is modified to fit with the data.<br></br>
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the given file or folder to reload</param>
        private void ReloadPointer(string path)
        {
            for (var i = 0; i < ChildrenFiles.Count; i++)
                if (ChildrenFiles[i].Path == path)
                {
                    ChildrenFiles[i].Dispose();
                    ChildrenFiles[i] = File.Exists(path)? new FilePointer.FilePointer(path) : new DirectoryPointer(path);
                    return;
                }
        }

        #endregion

        #region Delete

        /// <summary>
        ///     - Action : Change the current directory and remove children files <br></br>
        ///     - Implementation : NOT Check <br></br>
        /// </summary>
        /// <param name="dest">the destination file</param>
        /// <exception cref="SystemErrorException">System blocked system</exception>
        /// <exception cref="AccessException">The given dest folder could not be accessed</exception>
        /// <exception cref="PathNotFoundException">The given path does not exist</exception>
        public void ChangeDirectory(string dest)
        {
            if (Directory.Exists(dest))
            {
                var newPath = ManagerReader.ManagerReader.GetNameToPath(dest);
                //_watcher = new FileSystemWatcher(newPath);
                
                // Erase last directory
                foreach (var ft in ChildrenFiles) ft.Dispose();
                ChildrenFiles.Clear();
                
                // Replace
                try
                {
                    Directory.SetCurrentDirectory(newPath);
                    Path = newPath;
                    SetChildrenFiles();
                }
                catch (Exception e)
                {
                    if (e is ManagerException) throw;
                    throw e switch
                    {
                        ArgumentException or ArgumentNullException or PathTooLongException or FileNotFoundException
                            or DirectoryNotFoundException
                            => new PathNotFoundException("", ""),
                        IOException => new SystemErrorException("system blocked " + newPath, "ChangeDirectory"),
                        SecurityException or UnauthorizedAccessException => new AccessException(
                            newPath + " could not be accessed", "ChangeDirectory"),
                        _ => new ManagerException("", "", "", "", "")
                    };
                }

                return;
            }

            throw new PathNotFoundException(dest + " does not exist", "ChangeDirectory");
        }

        /// <summary>
        ///     Dispose the directory pointer
        /// </summary>
        public override void Dispose()
        {
            foreach (var pointer in _childrenFiles) pointer.Dispose();
            base.Dispose();
        }

        #endregion
    }
}