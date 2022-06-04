namespace LibraryClient
{
    public abstract class RemoteItem : IDisposable
    {
        /// <summary>
        /// Mandatory to access every information
        /// </summary>
        protected string _path;
        
        // Secondary to get other information on the item
        protected string _type;
        protected string _name;
        protected long _size;
        
        // Getter and Setter
        public string ParentPath(RemoteItem root) => _path == root.Path ? root.Path : _path.Remove(_path.Length - _name.Length, _name.Length);
        public string Path => _path;
        public string Type { get => _type; set => _type = value; }

        public string Name { get => _name; set => _name = value; }

        public long Size { get => _size; set => _size = value; }

        public bool IsDir;

        // CTOR
        public RemoteItem()
        {
            _path = "/";
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