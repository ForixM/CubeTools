using System.Collections.Generic;
using Library.ManagerReader;

namespace LibraryFTP
{
    
    public class FtpArboresence
    {
        private List<IFtpItem> _items;

        // public string ParentFolder; // TODO Implement
        public List<IFtpItem> Items => _items;

        public FtpArboresence()
        {
            _items = new List<IFtpItem>();
        }

        public override string ToString()
        {
            string str = "";
            foreach (IFtpItem ftpFile in _items)
            {
                str += ftpFile.ToString() + "\n";
            }

            return str;
        }
    }
}
