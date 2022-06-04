namespace  LibraryClient.LibraryFtp
{
    
    public class FtpFile : IFtpItem
    {
        public FtpFile(string name, int size, string parentPath, string lastmodified)
        {
            IsDir = false;
            this._name = name;
            this._size = size;
            this._parentPath = parentPath;
            this._lastmodified = lastmodified;
        }
    
        public FtpFile(string name, int size, IFtpItem parent, string lastmodified)
        {
            IsDir = false;
            this._name = name;
            this._size = size;
            this._parent = parent;
            this._lastmodified = lastmodified;
        }

        public override string ToString()
        {
            return $"{{name={_name}, size={_size}, path={Path}, lastmodified={_lastmodified}}}";
        }
    }
}
