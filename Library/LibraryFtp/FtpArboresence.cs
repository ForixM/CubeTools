using System.Collections.Generic;
using Library.ManagerReader;

namespace  Library.LibraryFtp
{
    
    public class FtpArboresence
    {
        private List<FtpPointer> _items;

        // public string ParentFolder; // TODO Implement
        public List<FtpPointer> Items => _items;

        public FtpArboresence()
        {
            _items = new List<FtpPointer>();
        }

        public override string ToString()
        {
            string str = "";
            foreach (FtpPointer ftpFile in _items)
            {
                str += ftpFile.ToString() + "\n";
            }

            return str;
        }
    }
}
