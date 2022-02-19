using System;

namespace Manager.ManagerExceptions
{
    public class PathNotFoundException : Exception // TODO Edit PathNotFoundException
    {
        public PathNotFoundException()
        {
            Console.Error.WriteLine("#######################################");
            Console.Error.WriteLine("### A PathNotFoundException occured ###");
            Console.Error.WriteLine(" # Low : the path does not exist");
        }

        public PathNotFoundException(string message) : this()
        {
            Console.Error.Write("  # error :");
            Console.Error.WriteLine(message);
        }

        public PathNotFoundException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at : ");
            Console.Error.WriteLine(func);
        }
    }
}