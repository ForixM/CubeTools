using Library.ManagerReader;
using Library;

namespace Library.LibraryFtp
{
    
    public abstract class FtpPointer : Pointer
    {
        protected FtpPointer _parent;

        public string SizeXaml => ManagerReader.ManagerReader.ByteToPowByte(Size);
        public string ParentPath => _parentPath;
        public FtpPointer Parent => _parent ?? new FtpFolder(_parentPath);
        // public new string Path => _parentPath == "/" && _name == "" ? _parentPath : (_parent == null ? _parentPath : _parent.Path) + _name + (IsDir ? "/" : "");
        
    }
}
