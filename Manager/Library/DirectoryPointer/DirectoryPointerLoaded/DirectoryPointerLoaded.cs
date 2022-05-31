using System.Security;
// Project's Library
using Library.ManagerExceptions;
using Timer = System.Threading.Timer;

namespace Library.DirectoryPointer
{
    /// <summary>
    ///     This class represents a pointer to a loaded directory
    ///     It will managed our file tree and will use actions to modify files efficiently
    /// </summary>
    public class DirectoryPointerLoaded : DirectoryPointer
    {
        #region Variables

        private List<Pointer> _childrenFiles;
        public List<Pointer> ChildrenFiles => _childrenFiles;

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
            _watcher = null;
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
            if (!Directory.Exists(path) || string.IsNullOrEmpty(path))
                throw new PathNotFoundException(path + " could not be identified", "Directory Constructor");

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
            try
            {
                SetChildrenFiles();
            }
            catch (Exception e)
            {
                if (e is ManagerException) Console.WriteLine("# ManagerException occured");
            }
        }

        #endregion

        #region Raising Events

        protected override void OnChanged(object sender, FileSystemEventArgs e)
        {
            ReloadPointer(e.FullPath);
        }

        /// <summary>
        ///     Raising event when a creation 
        /// </summary>
        protected override void OnCreated(object sender, FileSystemEventArgs e)
        {
            string created = e.FullPath.Replace("\\", "/");
            if (ManagerReader.ManagerReader.GetParent(created) == Path)
            {
                if (File.Exists(created)) _childrenFiles.Add(new FilePointer.FilePointer(created));
                else _childrenFiles.Add(new DirectoryPointer(created));
            }
        }

        protected override void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Remove(e.FullPath.Replace('\\','/'));
        }

        protected override void OnRenamed(object sender, RenamedEventArgs e)
        {
            var save = GetChild(e.OldFullPath);
            Remove(e.OldFullPath);
            save.Path = e.FullPath.Replace('\\','/');
            ChildrenFiles.Add(save);
            ReloadPointer(save.Path);
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
        ///     - Action : Remove every children in ChildrenFiles and set them
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
            
            foreach (var dir in Directory.GetDirectories(Path))
                ChildrenFiles.Add(new DirectoryPointer(dir.Replace('\\', '/')));
            foreach (var file in Directory.GetFiles(Path)) 
                ChildrenFiles.Add(new FilePointer.FilePointer(file.Replace('\\', '/')));
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
            if (HasChild(pointer.Path)) return;
            ChildrenFiles.Add(pointer);
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
        ///     - Action : Remove a file given with a path in the list of _childrenFiles
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the given path</param>
        public void Remove(string path)
        {
            for (var i = 0; i < ChildrenFiles.Count; i++)
            {
                if (ChildrenFiles[i].Path != path) continue;
                
                ChildrenFiles[i].Dispose();
                ChildrenFiles.RemoveAt(i);
                return;
            }
        }
        /// <summary>
        ///     - Action : Remove a file given with a path in the list of _childrenFiles
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="ft">the given pointer</param>
        public void Remove(Pointer ft)
        {
            for (var i = 0; i < ChildrenFiles.Count; i++)
                if (ChildrenFiles[i].Path == ft.Path)
                {
                    ChildrenFiles[i].Dispose();
                    ChildrenFiles.RemoveAt(i);
                    return;
                }
        }

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
                    if (File.Exists(path))
                    {
                        ChildrenFiles[i].Dispose();
                        ChildrenFiles[i] = new FilePointer.FilePointer(path);
                    }
                    else
                    {
                        ChildrenFiles[i].Dispose();
                        ChildrenFiles[i] = new DirectoryPointer(path);
                    }
                    return;
                }
        }

        #endregion

        #region Delete

        /// <summary>
        ///     -Action : Change the current directory and remove children files <br></br>
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
                _watcher = new FileSystemWatcher(newPath);
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
                catch (IOException)
                {
                    throw new SystemErrorException("system blocked " + newPath, "ChangeDirectory");
                }
                catch (SecurityException)
                {
                    throw new AccessException(newPath + " could not be accessed", "ChangeDirectory");
                }
                return;
            }

            throw new PathNotFoundException(dest + " does not exist", "ChangeDirectory");
        }

        /// <summary>
        ///     Delete function : delete the directory, its associated files in a high context level
        ///     Implementation : Not Check
        /// </summary>
        public override void Delete()
        {
            // Delete all files
            foreach (var pointer in ChildrenFiles) pointer.Delete();
            _childrenFiles = new List<Pointer>();
            // Delete directory
            ManagerWriter.ManagerWriter.DeleteDir(Path);
            Dispose();
        }

        /// <summary>
        ///     Dispose the directoryType to
        /// </summary>
        public override void Dispose()
        {
            foreach (var pointer in _childrenFiles) pointer.Dispose();
            base.Dispose();
        }

        #endregion
    }
}