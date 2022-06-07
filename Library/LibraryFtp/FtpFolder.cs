using Library.ManagerReader;

namespace Library.LibraryFtp
{
    
    public class FtpFolder : FtpPointer
    {
        public static readonly FtpFolder ROOT = new FtpFolder("", "/", "");

        public FtpFolder(string name, string parentPath, string lastModified)
        {
            IsDir = true;
            _name = name;
            if (parentPath == "/")
            {
                _path = "/";
                _parentPath = "/";
            }
            else
            {
                _path = parentPath + name + "/";
                string temp = parentPath;
                string name1 = "";
                if (temp[^1] == '/') temp = temp.Remove(temp.Length - 1);
                while (temp[^1] != '/')
                {
                    name1 = temp[^1] + name1;
                    temp = temp.Remove(temp.Length - 1);
                }

                _parentPath = temp;
            }
            _lastModified = lastModified;
        }
        
        public FtpFolder(string name, FtpPointer parent, string lastModified)
        {
            IsDir = true;
            _path = name =="" && parent.Path == "/" ? "/" :parent.Path + name + "/";
            _name = name;
            _parent = parent; 
            _parentPath = parent.Path;
            _lastModified = lastModified;
        }

        public FtpFolder(string path) // TODO Implement constructor with a path
        {
            _path = path;
            IsDir = true;
            if (path == "") path = "/";
            string newpath = path;
            while (newpath[^1] != '/') newpath = newpath.Remove(newpath.Length - 1);
            this._lastModified = "";
            string parentPath = path;
            if (parentPath == "/")
            {
                this._parentPath = "/";
                this._name = "";
            }
            else
            {
                string temp = parentPath;
                string name1 = "";
                if (temp[^1] == '/') temp = temp.Remove(temp.Length - 1);
                while (temp[^1] != '/')
                {
                    name1 = temp[^1] + name1;
                    temp = temp.Remove(temp.Length - 1);
                }

                this._name = name1;

                this._parentPath = temp;
            }
        }

        public override string ToString()
        {
            return $"{{name={_name}, lastmodified={_lastModified}}}";
        }
    }
}
