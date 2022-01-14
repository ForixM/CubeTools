using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;

namespace CubeTools.FileManager
{
    public class FileType
    {
        #region Variables

        private bool disposed;

        public static List<string> type = new List<string>()
            {"png", "pdf", "exe", "cs", "csv", ""};

        // Basics
        public string Path { get; set; }

        public string AbsPath { get; set; }

        public string Type { get; set; }

        // Properties
        public int Size { get; set; }

        public string Date { get; set; }

        public bool ReadOnly { get; set; }

        public bool Hidden { get; set; }

        // UI
        public Icon IconUI { get; set; }

        #endregion

        #region Init

        // Default Constructor
        // Constructors
        public FileType()
        {
            Path = null;
            AbsPath = null;
            Type = "";
            Size = 0;
            Date = null;
            Hidden = false;
            ReadOnly = false;
            IconUI = null;
        }

        public FileType(string path) : base()
        {
            Path = path;
        }

        public FileType(string path, string type) : base()
        {
            AbsPath = path;
            Type = type;
        }
        #endregion

        #region Delete


        // Destructor and Garbage Collector
        ~FileType()
        {
            FileWritter.SaveFileType(this);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                ////Number of instance you want to dispose
            }
        }
        #endregion
    }
}
