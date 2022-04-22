using System;
using System.IO;
using Library.ManagerExceptions;

namespace Library.Pointers
{
    public class FileType
    {
        #region Variables

        // This region contains every variables that stores the information of the file

        public static FileType NullPointer = new();
        // Basics
        private string _path;
        private string _name;
        private string _type;
        private long _size;

        // Date
        private string _date;
        private string _lastDate;
        private string _accessDate;

        // Attributes
        private bool _readOnly;

        private bool _hidden;
        private bool _compressed;
        private bool _archived;

        private bool _isDir;

        // Basics
        public string Path { get => _path; set => _path = value; }
        public string Name{ get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }
        public long Size { get => _size; set => _size = value; }
        public string SizeXaml => ManagerReader.ManagerReader.ByteToPowByte(Size);
        // Date
        public string Date { get => _date; set => _date = value; }
        public string LastDate{ get => _lastDate; set => _lastDate = value; }
        public string AccessDate{ get => _accessDate; set => _accessDate = value; }

        // Attributes
        public bool ReadOnly { get => _readOnly; set => _readOnly = value; }

        public bool Hidden { get => _hidden; set => _hidden = value; }
        public bool Compressed { get => _compressed; set => _compressed = value; }
        public bool Archived { get => _archived; set => _archived = value; }

        public bool IsDir { get => _isDir; set => _isDir = value; }

        #endregion

        #region Init

        // Constructors
        /// <summary>
        ///     - Action : Generate a null pointer
        ///     - Implementation : Check
        /// </summary>
        public FileType()
        {
            _path = "";
            _name = "";
            _type = "";
            _size = 0;
            _date = "";
            _lastDate = "";
            _accessDate = "";
            _hidden = false;
            _compressed = false;
            _archived = false;
            _readOnly = false;
            _isDir = false;
        }

        /// <summary>
        ///     - Action : Generate a pointer with it path only <br></br>
        ///     - Implementation : Check
        /// </summary>
        /// <param name="path">the path to load</param>
        /// <exception cref="PathNotFoundException">the given path does not exist</exception>
        public FileType(string path) : this()
        {
            path = path.Replace("\\", "/");
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                Dispose();
                throw new PathNotFoundException(path + " does not exist", "Constructor FileType");
            }
            // Path
            _path = path;
            // Primary
            _name = ManagerReader.ManagerReader.GetPathToName(_path);
            if (!IsDir)
                _size = ManagerReader.ManagerReader.GetFileSize(_path);
            else _size = 0;
            _isDir = Directory.Exists(_path);
            _type = ManagerReader.ManagerReader.GetFileExtension(_path);
            try
            {
                // Properties
                _readOnly = ManagerReader.ManagerReader.HasAttribute(FileAttributes.ReadOnly, _path);
                _hidden = ManagerReader.ManagerReader.HasAttribute(FileAttributes.Hidden, _path);
                _archived = ManagerReader.ManagerReader.HasAttribute(FileAttributes.Archive, _path);
                // Time
                _date = ManagerReader.ManagerReader.GetFileCreationDate(_path);
                _lastDate = ManagerReader.ManagerReader.GetFileLastEdition(_path);
                _accessDate = ManagerReader.ManagerReader.GetFileAccessDate(_path);
            }
            catch (ManagerException)
            {
            }
        }

        // Initializers

        #endregion

        #region Delete

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Operator

        private bool IsNull()
        {
            if (File.Exists(Name) || Directory.Exists(Name) || File.Exists(Path) || Directory.Exists(Path))
                return false;

            return true;
        }

        public static bool operator ==(FileType ft, FileType ft2)
        {
            if ((!File.Exists(ft.Path) || !Directory.Exists(ft.Path)) &&
                ft.IsNull() || (!File.Exists(ft2.Path) || !Directory.Exists(ft2.Path)) && ft.IsNull())
                return true;
            if (ft.Name != "" && ft2.Name != "")
            {
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

            return false;
        }

        public static bool operator !=(FileType ft, FileType ft2)
        {
            return !(ft == ft2);
        }

        protected bool Equals(FileType other)
        {
            return Path == other.Path && Name == other.Name && Type == other.Type && Size == other.Size &&
                   Date == other.Date && LastDate == other.LastDate && AccessDate == other.AccessDate &&
                   ReadOnly == other.ReadOnly && Hidden == other.Hidden && Compressed == other.Compressed &&
                   Archived == other.Archived && IsDir == other.IsDir;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FileType) obj);
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

        #endregion

        #region CLI

        public void PrintInformation()
        {
            Console.WriteLine("--FileType DEBUG--");
            Console.WriteLine("Path : " + Path);
            Console.WriteLine("Type : " + Type);
            Console.WriteLine("Size : " + Size);
            Console.WriteLine("Date : " + Date);
            Console.WriteLine("LastDate : " + LastDate);
            Console.WriteLine("AccessDate :" + AccessDate);
            Console.WriteLine("ReadOnly : " + ReadOnly);
            Console.WriteLine("Hidden : " + Hidden);
            Console.WriteLine("Directory : " + IsDir);
        }

        public void PrettyPrint()
        {
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");
            Console.WriteLine($"### Pointer to : {Path} ###");
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");

            Console.WriteLine($"## Name : {Name} ##  ## Type : {Type} ##");
            Console.WriteLine($"## Size : {Size} ##  ## Creation date : {Date} ##");
            Console.WriteLine($"## Last modification : {LastDate} ##");
            Console.WriteLine($"## Last access : {AccessDate} ##");
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");

            Console.WriteLine($"$$ Hidden : {Hidden} $$  $$ Is a directory : {IsDir}  $$");
            Console.WriteLine($"$$ ReadOnly : {ReadOnly} $$  $$ Archived : {Archived} $$");
            Console.WriteLine($"$$ Compressed : {Compressed} $$");
            foreach (var c in Path)
                Console.Write('_');
            Console.WriteLine("______________________");
        }

        #endregion
    }
}