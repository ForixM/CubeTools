using Reader = Manager.ManagerReader;
using Writer = Manager.ManagerWriter;
using System;
using System.Security.Principal;
using System.Linq;
using System.Security.AccessControl;
using System.Collections.Generic;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryType dt = new DirectoryType("C:/Users/mateo/OneDrive/Documents");
            FileType ft = Writer.CreateDir("test");
            FileType ft2 = Writer.CreateDir("test/test2");
            FileType ft3 = Writer.CreateDir("test3");
            List<FileType> list = new List<FileType>();
            list.Add(ft);
            list.Add(ft2);
            list.Add(ft3);
            Console.WriteLine(Writer.DeleteDir(list, true));
            dt.SetChildrenFiles();
        }
    }
}
