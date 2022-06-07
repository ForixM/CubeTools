﻿namespace Library
{
    public abstract class Pointer : IDisposable
    {
        /// <summary>
        /// Mandatory to access every information
        /// </summary>
        protected string _path;
        protected string _parentPath;
        
        // Secondary to get other information on the item
        protected string _type;
        protected string _name;
        protected long _size;
        protected string _lastModified;
        
        // Getter and Setter
        public string ParentPath => _parentPath;
        public string LastModified => _lastModified;

        public string Path
        {
            get => _path;
            set => _path = value;
        }
        public string Type
        {
            get => _type;
            set => _type = value;
        }

        public string Name 
        { 
            get => _name;
            set => _name = value;
        }

        public long Size
        {
            get => _size;
            set => _size = value;
        }


        public bool IsDir;

        // CTOR
        public Pointer()
        {
            _path = "/";
            _lastModified = "";
            _parentPath = "";
            _type = "";
            _name = "";
            _size = 0;
            IsDir = false;
        }

        public Pointer(string path) : this()
        {
            _path = path;
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}