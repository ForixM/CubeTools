using System;

namespace Manager.ManagerExceptions
{
    public class PathNotFoundException : Exception // TODO Edit PathNotFoundException
    {
        public PathNotFoundException()
        {
            Console.Error.WriteLine("### A PathNotFoundException occured ###");
        }

        public PathNotFoundException(string message) : base()
        {
            Console.Error.Write("# output : ");
            Console.Error.WriteLine(message);
        }
    }
}