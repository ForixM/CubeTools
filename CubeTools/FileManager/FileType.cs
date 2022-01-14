using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeTools.FileManager
{
    internal class FileType
    {
        public enum Type
        {
            PDF,
            EXE,
            TXT,
            CS
        }
        private string _path;
        private string _abs_path;
        private Type _type;
        private int _size;
        private string _date;
        private bool _hide;
        private bool _readOnly;

        public string ReformatPath(string path)
        {
            int i = path.Length;
            string returnPath = "";
            while (i > 0 && path[i] != '\\')
            {
                returnPath = returnPath + path[i];
            }

            return returnPath;
        }

        public string GetSinglePath(string abs_path)
        {
            return null;
        }
    }
}
