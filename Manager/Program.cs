using Reader = Manager.ManagerReader;
using Writer = Manager.ManagerWriter;
using System;
using System.Security.Principal;
using System.Linq;
using System.Security.AccessControl;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommandLine commandline = new CommandLine("C:/Users/mateo/OneDrive");
            commandline.Process();
        }
    }
}
