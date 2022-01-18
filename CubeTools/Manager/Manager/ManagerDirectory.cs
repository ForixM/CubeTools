using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class DirectoryType
    {
        private string _path;
        private string _abs_path;
        public List<FileType> _children_files;
        private int _size;
        private string _date;
        private bool _hide;
        private bool _readOnly;

        // Constructors
        DirectoryType()
        {
            _path = "";
            _abs_path = "";
            _children_files = new List<FileType>();
            _size = 0;
            _hide = false;
            _readOnly = false;
            _date = "";
        }

        DirectoryType(string path) : base() => _path = path;
        // Methods
        public void ChildrenFiles()
        {
            Directory.GetFiles(_abs_path);
        }

        public void ChangeDirectory()
        {
            foreach (var file in _children_files)
            {
                ManagerWriter.SaveFileType(file);
                file.Dispose();
            }
        }

        // Function
        public string[] GetChildrenFiles(string dirPath)
        {
            return Directory.GetFiles(dirPath);
        }
    }
}
