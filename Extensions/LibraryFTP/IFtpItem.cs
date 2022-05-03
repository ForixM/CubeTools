using Library.ManagerReader;
using Library.ManagerWriter;

namespace LibraryFTP
{
    
    public abstract class IFtpItem
    {
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
        public string Path => _parentPath + _name;

        public string LastModified => _lastmodified;
    }
}
