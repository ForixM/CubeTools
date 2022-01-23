using Reader = Manager.ManagerReader;
using Writer = Manager.ManagerWriter;
using System;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CommandLine commandline = new CommandLine("C:/");
            commandline.Process();
        }
    }
}
