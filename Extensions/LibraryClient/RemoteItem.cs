namespace LibraryClient
{
    public abstract class RemoteItem
    {
        /// <summary>
        /// Mandatory to access every information
        /// </summary>
        protected string _path;
        
        // Secondary to get other information on the item
        protected string _type;
        protected string _name;
        protected long _size;
        protected bool _isDir;

        // Getter and Setter

        public string ParentPath(RemoteItem root) => _path == root.Path ? root.Path : _path.Remove(_path.Length - _name.Length, _name.Length);
        public string Path => _path;
        public string Type { get => _type; set => _type = value; }

        public string Name { get => _name; set => _name = value; }

        public long Size { get => _size; set => _size = value; }

        public bool IsDir => _isDir;

        // CTOR
        public RemoteItem()
        {
            _path = "/";
            _type = "";
            _name = "";
            _size = 0;
            _isDir = false;
        }

        public RemoteItem(string path) : this()
        {
            _path = path;
        }
    }
}