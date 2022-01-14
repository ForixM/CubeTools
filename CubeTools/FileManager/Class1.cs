using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CubeTools.FileManager
{
    internal class FileReader
    {
        bool IsFile(string path)
        {
            return File.Exists(path);
        }
    }
}
