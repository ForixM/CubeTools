using System;

namespace Manager.ManagerExceptions
{
    public class ReplaceException : Exception
    {
        public ReplaceException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###    A ReplaceException occured    ###");
            Console.Error.WriteLine("  # Low : Consider replacing file or directory #");
        }

        public ReplaceException(string message) : this()
        {
            Console.Error.Write("  # error : ");
            Console.Error.WriteLine(message);
        }

        public ReplaceException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at : ");
            Console.Error.WriteLine(func);
        }
    }
}