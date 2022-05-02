namespace LibraryFTP
{
    
    public class FtpFile : IFtpItem
    {
        // private string _name;
        // private string _parentPath;

        // public string Name => _name;
        // public string ParentPath => _parentPath;
        // public string Path => _parentPath + _name;
    
        public FtpFile(string name, int size, string parentPath, string lastmodified)
        {
            this._name = name;
            this._size = size;
            this._parentPath = parentPath;
            this._lastmodified = lastmodified;
        }

        public override string ToString()
        {
            return $"{{name={_name}, size={_size}, path={Path}, lastmodified={_lastmodified}}}";
        }
    }
}
