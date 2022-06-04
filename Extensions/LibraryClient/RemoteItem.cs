namespace LibraryClient
{
    public abstract class RemoteItem : IDisposable
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
        public string Path => _path;
        public string Type => _type;
        public string Name => _name;
        public long Size => _size;
        

        public bool IsDir;

        // CTOR
        public RemoteItem()
        {
            _path = "/";
            _lastModified = "";
            _parentPath = "";
            _type = "";
            _name = "";
            _size = 0;
            IsDir = false;
        }

        public RemoteItem(string path) : this()
        {
            _path = path;
        }

        public void Dispose() => GC.SuppressFinalize(this);
    }
}