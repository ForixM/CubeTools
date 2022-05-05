using Library.ManagerReader;
using Library.ManagerWriter;

namespace LibraryFTP
{
    
    public abstract class IFtpItem
    {
        protected IFtpItem _parent;
        protected string _parentPath;
        protected string _name;
        protected long _size;
        protected string _sizeXaml;
        protected string _type;
        protected string _lastmodified;

        public bool IsDir => this is FtpFolder;
        public long Size => _size;
        public string SizeXaml => ManagerReader.ByteToPowByte(Size);
        public string Name => _name;
        public string Type => _type;
        public string ParentPath => _parentPath;
        public IFtpItem Parent => _parent ?? new FtpFolder(_parentPath);
        public string Path => _parentPath == "/" && _name == "" ? _parentPath : (_parent == null ? _parentPath : _parent.Path) + _name + (IsDir ? "/" : "");

        public string LastModified => _lastmodified;
    }
}
