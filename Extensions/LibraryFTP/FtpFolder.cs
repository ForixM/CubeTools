namespace LibraryFTP
{
    
    public class FtpFolder : IFtpItem
    {
        // private string _parentPath;
        // public string Name => _name;
        // public string ParentPath => _parentPath;
        // public string Path => _parentPath + _name;

        public static FtpFolder ROOT = new FtpFolder("", "/", "");
    
        public FtpFolder(string name, string parentPath, string lastmodified)
        {
            this._name = name;
            this._parentPath = parentPath;
            this._lastmodified = lastmodified;
        }

        public FtpFolder(string path) // TODO Implement constructor with a path
        {
            this._name = "";
            this._parentPath = "";
        }

        public override string ToString()
        {
            return $"{{name={_name}, lastmodified={_lastmodified}}}";
        }
    }
}
