using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class FileType
    {
        protected bool Equals(FileType other)
        {
            return Path == other.Path && Name == other.Name && Type == other.Type && Size == other.Size && Date == other.Date && LastDate == other.LastDate && AccessDate == other.AccessDate && ReadOnly == other.ReadOnly && Hidden == other.Hidden && Compressed == other.Compressed && Archived == other.Archived && IsDir == other.IsDir;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
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

        #region Variables

        // This region contains every variables that stores the information of the file

        public static FileType NullPointer = new FileType();

        public static List<string> type = new List<string>()
            {"png", "pdf", "exe", "cs", "csv", ""};

        // Basics
        public string Path { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }

        // Date
        public string Date { get; set; }
        public string LastDate { get; set; }
        public string AccessDate { get; set; }

        // Attributes
        public bool ReadOnly { get; set; }

        public bool Hidden { get; set; }
        public bool Compressed { get; set; }
        public bool Archived { get; set; }
        public bool IsDir { get; set; }

        #endregion

        #region Init

        // Constructors
        // Implementation : Check
        public FileType()
        {
            Path = "";
            Name = "";
            Type = "";
            Size = 0;
            Date = "";
            LastDate = "";
            AccessDate = "";
            Hidden = false;
            Compressed = false;
            Archived = false;
            ReadOnly = false;
            IsDir = false;
        }

        // Implementation : Check
        public FileType(string path) : this()
        {
            this.Path = path;
            if (File.Exists(path) || Directory.Exists(path))
            {
                Name = ManagerReader.GetPathToName(path);
            }
            else
            {
                this.Dispose();
            }

        }

        // Initializers
        // Implemented Check
        public static void Init(ref FileType ft)
        {
            ManagerReader.ReadFileType(ref ft);
        }

        #endregion

        #region Delete

        // This region is not completed, memory will be fixed later

        // Destructor and Garbage Collector
        ~FileType()
        {
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Operator

        private bool IsNull()
        {
            if (File.Exists(this.Name) || Directory.Exists(this.Name) || File.Exists(this.Path) || Directory.Exists(this.Path))
            {
                return false;
            }

            return true;
        }
        public static bool operator ==(FileType ft, FileType ft2)
        {
            if (ft is null && ft2 is null)
                return true;
            if ((!File.Exists(ft.Path) || !Directory.Exists(ft.Path)) &&
                ft.IsNull() || (!File.Exists(ft2.Path) || !Directory.Exists(ft2.Path)) && ft.IsNull())
            {
                return true;
            }
            if (ft.Name != "" && ft2.Name != null)
            {
                bool res = true;
                res &= (ft.Path == ft2.Path);
                res &= (ft.Name == ft2.Name);
                res &= (ft.Size == ft2.Size);
                res &= (ft.Date == ft2.Date);
                res &= (ft.LastDate == ft2.LastDate);
                res &= (ft.AccessDate == ft2.AccessDate);
                res &= (ft.Hidden == ft2.Hidden);
                res &= (ft.Compressed == ft2.Compressed);
                res &= (ft.Archived == ft2.Archived);
                res &= (ft.ReadOnly == ft2.ReadOnly);
                res &= (ft.IsDir == ft2.IsDir);
                return res;
            }

            return false;
        }

        public static bool operator !=(FileType ft, FileType ft2)
        {
            return !(ft == ft2);
        }

        #endregion

        #region Debug

        public void PrintInformation()
        {
            Console.WriteLine("--FileType DEBUG--");
            Console.WriteLine("Path : " + this.Path);
            Console.WriteLine("Type : " + this.Type);
            Console.WriteLine("Size : " + this.Size);
            Console.WriteLine("Date : " + this.Date);
            Console.WriteLine("LastDate : " + this.LastDate);
            Console.WriteLine("AccessDate :" + this.AccessDate);
            Console.WriteLine("ReadOnly : " + this.ReadOnly);
            Console.WriteLine("Hidden : " + this.Hidden);
            Console.WriteLine("Directory : " + this.IsDir);
        }

        #endregion
    }
}