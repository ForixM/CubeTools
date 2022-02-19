using System;

namespace Manager.ManagerExceptions
{
    public class AccessException : Exception // TODO Edit Access Exception
    {
        public AccessException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###    An AccessException occured    ###");
            Console.Error.WriteLine("  # Medium : CubeTools cannot access your files or directories");
        }

        public AccessException(string message) : this()
        {
            Console.Error.Write("# error : ");
            Console.Error.WriteLine(message);
        }

        public AccessException(string message, string func) : this(message)
        {
            Console.Error.Write("# error at : ");
            Console.Error.WriteLine(func);
        }
    }
}