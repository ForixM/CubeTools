using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CubeTools.FileManager
{
    internal class FileReader
    {

        // IsFile : Returns if the given path is a file 
        public bool IsFile(string path)
        {
            return File.Exists(path);
        }

        // IsDirectory : Returns if the given path is a directory
        public bool IsDirectory(string path)
        {
            return Directory.Exists(path);
        }


    }
}
