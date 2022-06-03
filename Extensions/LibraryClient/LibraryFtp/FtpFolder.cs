using Library.ManagerReader;

namespace LibraryClient.LibraryFtp
{
    
    public class FtpFolder : IFtpItem
    {

        public static readonly FtpFolder ROOT = new FtpFolder("", "/", "");

        public FtpFolder(string name, string parentPath, string lastmodified)
        {
            this._name = name;
            if (parentPath == "/")
            {
                this._parentPath = "/";
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

                this._parentPath = temp;
            }
            this._lastmodified = lastmodified;
        }
        
        public FtpFolder(string name, IFtpItem parent, string lastmodified)
        {
            this._name = name;
            this._parent = parent;
            this._parentPath = parent.Path;
            this._lastmodified = lastmodified;
        }

        public FtpFolder(string path) // TODO Implement constructor with a path
        {
            if (path == "") path = "/";
            string newpath = path;
            while (newpath[^1] != '/') newpath = newpath.Remove(newpath.Length - 1);
            this._lastmodified = "";
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
            return $"{{name={_name}, lastmodified={_lastmodified}}}";
        }
    }
}
