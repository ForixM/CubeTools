namespace  Library.LibraryFtp
{
    
    public class FtpFile : FtpPointer
    {
        public FtpFile(string name, long size, string parentPath, string lastModified)
        {
            IsDir = false;
            _path = parentPath + name;
            _name = name;
            _size = size;
            _parentPath = parentPath;
            _lastModified = lastModified;
        }
    
        public FtpFile(string name, long size, FtpPointer parent, string lastModified)
        {
            IsDir = false;
            _name = name;
            _size = size;
            _parent = parent;
            _lastModified = lastModified;
            _path = parent.Path + name;
        }
        
        public override string ToString() => $"{{name={_name}, size={_size}, path={Path}, lastmodified={_lastModified}}}";
    }
}
