// System's Library
using System.Security;
// Project's Library
using Library.ManagerExceptions;
using Timer = System.Threading.Timer;

namespace Library.Pointers
{
    /// <summary>
    ///     This class represents a pointer to a loaded directory
    ///     It will managed our file tree and will use actions to modify files efficiently
    /// </summary>
    public class DirectoryType
    {
        #region Debug

        public void PrintInformation()
        {
            Console.WriteLine("----------------------");
            Console.WriteLine("Directory : " + Path);
            Console.WriteLine("----------------------");
            foreach (var child in ChildrenFiles)
                child.PrintInformation();
        }

        #endregion

        #region Variables

        // This region contains every variables of the DirectoryType class

        // Attributes

        // Watcher

        // Getter and Setter
        public string Path { get; set; }

        public string Name { get; set; }

        public List<FileType> ChildrenFiles { get; set; }

        public long Size { get; private set; }

        public string Date { get; }

        public string LastDate { get; }

        public string AccessDate { get; }

        public bool Hidden { get; }

        public bool ReadOnly { get; }

        public FileSystemWatcher Watcher { get; private set; }

        #endregion

        #region Init

        // This region will generate correctly the DirectoryType with FileTypes
        
        
        /// <summary>
        ///     - Action : Default constructor of DirectoryType class
        ///     - Implementation : Check
        /// </summary>
        public DirectoryType()
        {
            Name = "";
            Path = "";
            ChildrenFiles = new List<FileType>();
            Size = 0;
            Date = "";
            LastDate = "";
            AccessDate = "";
            Hidden = false;
            ReadOnly = false;
            Watcher = null;
        }

        /// <summary>
        ///     - Action : Load a pointer to a directory given in the parameter with its path
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path of the directory</param>
        /// <exception cref="AccessException">the given directory cannot be accessed</exception>
        /// <exception cref="SystemErrorException">The system blocked the constructor</exception>
        /// <exception cref="ManagerException">An error occured</exception>
        public DirectoryType(string path)
        {
            // Verify if the directory already exists and its path is correct
            path = path.Replace('\\', '/');
            if (Directory.Exists(path) && !string.IsNullOrEmpty(path))
            {
                try
                {
                    Directory.SetCurrentDirectory(path);
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
                Path = path;
                Name = ManagerReader.ManagerReader.GetPathToName(path);
                ChildrenFiles = new List<FileType>();
                try
                {
                    // Set attributes
                    Date = ManagerReader.ManagerReader.GetFileCreationDate(path);
                    LastDate = ManagerReader.ManagerReader.GetFileLastEdition(path);
                    AccessDate = ManagerReader.ManagerReader.GetFileAccessDate(path);
                    Size = ManagerReader.ManagerReader.GetFileSize(path);
                    Hidden = ManagerReader.ManagerReader.IsFileHidden(path);
                    ReadOnly = ManagerReader.ManagerReader.IsReadOnly(path);
                    // Set Sub-Files/Folder
                    SetChildrenFiles();
                }
                catch (Exception e)
                {
                    if (e is ManagerException)
                        Console.WriteLine("# ManagerException occured");
                    else throw;
                }
                // Watcher
                Watcher = new FileSystemWatcher(path);
                Watcher.Changed += OnChanged;
                Watcher.Created += OnCreated;
                Watcher.Deleted += OnDeleted;
                Watcher.Renamed += OnRenamed;
                Watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName
                                       | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
                Watcher.Filter = "*";
                Watcher.IncludeSubdirectories = true;
                Watcher.EnableRaisingEvents = true;
                
                // Launch watcher
                StartRecurringWatcher();
            }
            else
            {
                throw new PathNotFoundException(path + " could not be identified", "Directory Constructor");
            }
        }

        #endregion

        #region LocalWatcher

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            ReloadPointer(e.FullPath);
        }

        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            ChildrenFiles.Add(ManagerReader.ManagerReader.ReadFileType(e.FullPath.Replace('\\','/')));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Hello");
            Remove(e.FullPath.Replace('\\','/'));
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            var save = GetChild(e.OldFullPath);
            Remove(e.OldFullPath);
            save.Path = e.FullPath.Replace('\\','/');
            ChildrenFiles.Add(save);
            ReloadPointer(save.Path);
        }

        #endregion 
        
        #region GlobalWatcher

        private Timer _timer;
        private int _interval = 4000;
        
        private void StartRecurringWatcher()
        {
            _timer = new Timer(TickWatcher, null, _interval, Timeout.Infinite);
        }

        public void StopRecurringWatcher()
        {
            _timer.DisposeAsync();
            _timer = null;
        }

        private void TickWatcher(object state)
        {
            try
            {
                if (ManagerReader.ManagerReader.GetAmountOfLocalData(this.Path) != this.ChildrenFiles.Count)
                    SetChildrenFiles();
            }
            finally
            {
                _timer?.Change(_interval, Timeout.Infinite);
            }
        }

        #endregion

        #region FileType

        // This region contains every functions that can have access to fileType through directoryType

        /// <summary>
        ///     This function creates a FileType and return it using a path
        ///     Implementation : NOT Check
        /// </summary>
        /// <exception cref="AccessException">the data cannot be read</exception>
        /// <exception cref="Exception">an unknown exception occured</exception>
        /// <exception cref="InUseException">the data in being used by another program</exception>
        public FileType GetChild(string path)
        {
            var res = ChildrenFiles.FindAll(ft => ft.Path == path);
            return res.Count == 0 ? ManagerReader.ManagerReader.ReadFileType(path) : res[0];
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
            foreach (var file in ChildrenFiles) file.Dispose();
            ChildrenFiles.Clear();
            foreach (var file in Directory.GetFiles(Path)) 
                ChildrenFiles.Add(new FileType(file.Replace('\\', '/')));
            foreach (var dir in Directory.GetDirectories(Path))
                ChildrenFiles.Add(new FileType(dir.Replace('\\', '/')));
        }

        /// <summary>
        ///     - Action : Verify if the file or folder given with its path is a child of the directory
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">The given path to verify</param>
        /// <returns>if the directoryType has the given child</returns>
        public bool HasChild(string path)
        {
            foreach (var ft in ChildrenFiles)
                if (ft.Path == path)
                    return true;

            return false;
        }

        /// <summary>
        /// Add single child to ChildrenFiles
        /// </summary>
        /// <param name="ft"></param>
        public void AddChild(FileType ft)
        {
            if (HasChild(ft.Path))
                return;
            ChildrenFiles.Add(ft);
        }
        
        /// <summary>
        /// Overload of <see cref="AddChild(Library.Pointers.FileType)"/>
        /// </summary>
        /// <param name="path">Absolute Path</param>
        /// <param name="isdir">Whether it is a directory or not</param>
        public void AddChild(string path, bool isdir = false)
        {
            if (HasChild(path))
                return;
            if (isdir)
                AddDir(ManagerReader.ManagerReader.GetPathToName(path));
            else
                AddFile(ManagerReader.ManagerReader.GetPathToName(path), ManagerReader.ManagerReader.GetFileExtension(path));
        }

        /// <summary>
        ///     - Action : Add a file and create it to the directory
        /// </summary>
        /// <param name="name">the name of the file with no extension</param>
        /// <param name="extension">the extension of the file</param>
        public void AddFile(string name, string extension)
        {
            var path = $"{Path}/{name}.{extension}";
            ManagerWriter.ManagerWriter.Create(path, extension);
            var ft = ManagerReader.ManagerReader.ReadFileType(path);
            ChildrenFiles.Add(ft);
        }

        /// <summary>
        ///     - Action : Create a directory and add it to the current loaded directory <br></br>
        ///     - Implementation : NOT Check <br></br>
        /// </summary>
        /// <param name="name"></param>
        public void AddDir(string name)
        {
            var path = Path + '/' + name;
            ManagerWriter.ManagerWriter.CreateDir(path);
            var ft = ManagerReader.ManagerReader.ReadFileType(path);
            ManagerReader.ManagerReader.ReadFileType(ref ft);
            ChildrenFiles.Add(ft);
        }

        /// <summary>
        ///     - Action : Remove a file given with a path in the list of _childrenFiles
        ///     - Implementation : NOT Check
        /// </summary>
        /// <param name="path">the given path</param>
        public void Remove(string path)
        {
            for (var i = 0; i < ChildrenFiles.Count; i++)
                if (ChildrenFiles[i].Path == path)
                {
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
        public void Remove(FileType ft)
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
                    ChildrenFiles[i] = ManagerReader.ManagerReader.ReadFileType(path);
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
                Watcher = new FileSystemWatcher(newPath);
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
                // Restart Recurring Watcher
                StopRecurringWatcher();
                StartRecurringWatcher();
                return;
            }

            throw new PathNotFoundException(dest + " does not exist", "ChangeDirectory");
        }

        /// <summary>
        ///     Delete function : delete the directory, its associated files in a high context level
        ///     Implementation : Not Check
        /// </summary>
        public void Delete()
        {
            // Delete all files
            foreach (var ft in ChildrenFiles) ManagerWriter.ManagerWriter.Delete(ft);
            ChildrenFiles = new List<FileType>();
            // Delete directory
            ManagerWriter.ManagerWriter.DeleteDir(Path, false);
            Path = "";
            Size = 0;
            Dispose();
        }

        /// <summary>
        ///     Dispose the directoryType to
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Operator

        private bool IsNull()
        {
            return !Directory.Exists(Path);
        }

        public static bool operator ==(DirectoryType dir1, DirectoryType dir2)
        {
            return Equals(dir1, dir2);
        }

        public static bool operator !=(DirectoryType dir1, DirectoryType dir2)
        {
            return !Equals(dir1, dir2);
        }

        // Others
        protected bool Equals(DirectoryType other)
        {
            return Path == other.Path && Name == other.Name && Equals(ChildrenFiles, other.ChildrenFiles)
                   && Size == other.Size && Date == other.Date && LastDate == other.LastDate
                   && AccessDate == other.AccessDate && Hidden == other.Hidden && ReadOnly == other.ReadOnly;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((DirectoryType) obj);
        }

        #endregion

        #region CommandLine

        // NO NEED TO BE IMPLEMENTED, DEBUG FUNCTIONS

        /// <summary>
        ///     Display all children of the current directory pointer
        /// </summary>
        public void DisplayChildren()
        {
            Console.WriteLine();
            Console.WriteLine("LastWriteTime               Size                     Name");
            Console.WriteLine("-------------               ----                     ----");
            foreach (var file in ChildrenFiles)
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
            var msg = value;
            var maxSize = 0;
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

            for (var i = 0; i < maxSize - value.Length; i++) msg += " ";

            return msg;
        }

        #endregion
    }
}