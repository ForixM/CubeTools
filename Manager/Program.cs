using System;
using System.Configuration;
using System.Collections.Specialized;
using System.Threading;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0 && args[0] == "cli")
                RunCLI.Launch();

            foreach (var e in ManagerReader.ManagerReader.RecommendedProgramsWindows("h"))
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("Type enter to stop : ");
            Console.ReadLine();
        }
    }
}