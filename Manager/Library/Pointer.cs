using Library.ManagerExceptions;

namespace Library
{

    /// <summary>
    /// ##################### POINTER ABSTRACT CLASS ######################## <br></br>
    /// ALL POINTERS ARE SUPPOSED TO GET THE FOLLOWING INFORMATION <br></br>
    /// CUBETOOLS SOFTWARE DEPENDS ON THIS CLASS <br></br>
    /// Purpose : Get and store all necessary information about a file or <br></br>
    /// a folder physically stored in the client's system. <br></br>
    /// Inheritance : x - FilePointer, DirectoryPointer
    /// ##################################################################
    /// </summary>
    public abstract partial class Pointer : IDisposable
    {
        // This region contains every variables that stores the information of the file
        #region Variables
        
        public static FilePointer.FilePointer NullPointer = new();
        // Basics
        protected string _path;
        protected string _name;
        protected string _type;
        protected long _size;

        // Others
        protected bool _isSizeLoaded;

        // Basics
        public object Client;
        public string Path { get => _path; set => _path = value; }
        public string Name{ get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }
        public long Size { get => _size; set => _size = value; }
        public string SizeXaml => ManagerReader.ManagerReader.ByteToPowByte(Size);
        
        // Date
        public string Date => GetPointerCreationDate();

        public string LastDate => GetPointerLastEdition();
        public string AccessDate => GetPointerAccessDate();

        // Attributes
        public bool ReadOnly => HasAttribute(FileAttributes.ReadOnly);
        public bool Hidden => HasAttribute(FileAttributes.Hidden);
        public bool Compressed => HasAttribute(FileAttributes.Compressed);
        public bool Archived => HasAttribute(FileAttributes.Archive);

        public bool IsDir => Directory.Exists(_path);
        

        #endregion

        // This region operates the creation of a filePointer
        #region Init
        
        /// <summary>
        ///     - Action : Generate a null pointer <br></br>
        ///     - Implementation : Check
        /// </summary>
        public Pointer()
        {
            _path = "";
            _name = "";
            _type = "";
            _size = 0;
            _isSizeLoaded = false;
        }

        /// <summary>
        ///     - Action : Generate a pointer with it path only <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path to load</param>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        public Pointer(string path) : this()
        {
            path = path.Replace("\\", "/");
            if (!File.Exists(path) && !Directory.Exists(path))
            { 
                Dispose();
                throw new PathNotFoundException(path + " does not exist", "Constructor FilePointer");
            }
            // Path
            _path = path;
            // Necessary
            _name = ManagerReader.ManagerReader.GetPathToName(_path);
            _size = _isSizeLoaded ? ManagerReader.ManagerReader.GetFileSize(_path) : 0;
            _type = ManagerReader.ManagerReader.GetFileExtension(_path);
        }
        
        public virtual void Dispose() => GC.SuppressFinalize(this);

        #endregion

        // This region contains operators
        #region Operator
        
        public static bool operator ==(Pointer ft, Pointer ft2)
        {
            if ((!File.Exists(ft.Path) || !Directory.Exists(ft.Path)) &&
                ft.IsNull() || (!File.Exists(ft2.Path) || !Directory.Exists(ft2.Path)) && ft.IsNull())
                return true;
            if (ft.Name == "" || ft2.Name == "") return false;
            
            var res = true;
            res &= ft.Path == ft2.Path;
            res &= ft.Name == ft2.Name;
            res &= ft.Size == ft2.Size;
            res &= ft.Date == ft2.Date;
            res &= ft.LastDate == ft2.LastDate;
            res &= ft.AccessDate == ft2.AccessDate;
            res &= ft.Hidden == ft2.Hidden;
            res &= ft.Compressed == ft2.Compressed;
            res &= ft.Archived == ft2.Archived;
            res &= ft.ReadOnly == ft2.ReadOnly;
            res &= ft.IsDir == ft2.IsDir;
            return res;

        }

        public static bool operator !=(Pointer ft, Pointer ft2) => !(ft == ft2);

        protected bool Equals(Pointer other) => Path == other.Path && Name == other.Name && Type == other.Type && Size == other.Size &&
                                                Date == other.Date && LastDate == other.LastDate && AccessDate == other.AccessDate &&
                                                ReadOnly == other.ReadOnly && Hidden == other.Hidden && Compressed == other.Compressed &&
                                                Archived == other.Archived && IsDir == other.IsDir;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Pointer) obj);
        }

        public override int GetHashCode()
        {
            var hashCode = new HashCode();
            hashCode.Add(Path);
            hashCode.Add(Name);
            hashCode.Add(Type);
            hashCode.Add(Size);
            hashCode.Add(Date);
            hashCode.Add(LastDate);
            hashCode.Add(AccessDate);
            hashCode.Add(ReadOnly);
            hashCode.Add(Hidden);
            hashCode.Add(Compressed);
            hashCode.Add(Archived);
            hashCode.Add(IsDir);
            return hashCode.ToHashCode();
        }
        
        public bool IsNull() => !File.Exists(Name) && !Directory.Exists(Name) && !File.Exists(Path) && !Directory.Exists(Path);

        #endregion
    }
}