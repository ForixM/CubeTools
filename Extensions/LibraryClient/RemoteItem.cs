namespace LibraryClient
{
    public abstract class RemoteItem
    {
        protected string _type;
        protected string _name;
        protected long _size;
        protected bool _isDir;
        
        
        public string Type => _type;
        public string Name => _name;
        public long Size => _size;
        public bool IsDir => _isDir;

        public RemoteItem()
        {
            _type = "";
            _name = "";
            _size = 0;
            _isDir = false;
        }
    }
}