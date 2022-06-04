using Library.ManagerReader;
using LibraryClient;

namespace LibraryClient.LibraryFtp
{
    
    public abstract class IFtpItem : RemoteItem
    {
        protected IFtpItem _parent;
        protected string _parentPath;
        protected string _lastmodified;
        
        public string SizeXaml => ManagerReader.ByteToPowByte(Size);
        public string ParentPath => _parentPath;
        public IFtpItem Parent => _parent ?? new FtpFolder(_parentPath);
        public new string Path => _parentPath == "/" && _name == "" ? _parentPath : (_parent == null ? _parentPath : _parent.Path) + _name + (IsDir ? "/" : "");

        public string LastModified => _lastmodified;
    }
}
