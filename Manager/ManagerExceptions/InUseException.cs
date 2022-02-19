using System;

namespace Manager.ManagerExceptions
{
    public class InUseException : Exception // TODO Edit InUseException
    {
        public InUseException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###     A InUseException occured     ###");
            Console.Error.WriteLine("  # Low : a program is using the accessed file or directory");
        }

        public InUseException(string message) : this()
        {
            Console.Error.Write("  # error :");
            Console.Error.WriteLine(message);
        }

        public InUseException(string message, string func) : this(message)
        {
            Console.Error.WriteLine("  # error at :" + func);
        }

        public InUseException(string message, string func, string program) : this(message, func)
        {
            Console.Error.WriteLine("  # program :" + program);
        }
    }
}