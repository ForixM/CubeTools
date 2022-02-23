using Reader = Manager.ManagerReader;
using Writer = Manager.ManagerWriter;
using System;
using System.Security.Principal;
using System.Linq;
using System.Security.AccessControl;
using System.Collections.Generic;
using System.IO;
using Manager.ManagerExceptions;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ManagerCLI cl = new ManagerCLI(Directory.GetCurrentDirectory());
            cl.Process();
        }
    }
}