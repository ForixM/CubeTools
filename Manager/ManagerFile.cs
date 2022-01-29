using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class FileType
    {
        #region Variables

        // Use for dynamic allocation of the class
        private bool _disposed;

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
        public string AccessDate { get; set;}

        // Attributes
        public bool ReadOnly { get; set; }

        public bool Hidden { get; set; }
        public bool IsDir { get; set; }
        

        #endregion

        #region Init

        // Constructors
        // Implemented Check
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
            ReadOnly = false;
            IsDir = false;
        }

        // Implemented Check
        public FileType(string path) : this()
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                Name = System.IO.Path.GetFileName(path);
            }
            Path = System.IO.Path.GetFullPath(path);
        }

        // Initializers
        // Implemented Check
        public void Init(ref FileType ft)
        {
            ManagerReader.ReadFileType(ref ft);
        }

        #endregion

        #region Delete


        // Destructor and Garbage Collector
        ~FileType()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }

            if (disposing)
            {
                ////Number of instance you want to dispose
            }
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
            Console.WriteLine("AccessDate :"  + this.AccessDate);
            Console.WriteLine("ReadOnly : " + this.ReadOnly);
            Console.WriteLine("Hidden : " + this.Hidden);
            Console.WriteLine("Directory : " + this.IsDir);
    }
        
        #endregion
    }
}
