using Reader = Manager.ManagerReader;
using Writer = Manager.ManagerWriter;
using System;
using System.Security.Principal;
using System.Linq;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryType env = new DirectoryType("C:/Users/mateo/OneDrive/Documents/GitHub/CubeTools/ManagerTests/Tests/ReaderTests");
            env.AddFile("test", "txt");
        }
    }
}