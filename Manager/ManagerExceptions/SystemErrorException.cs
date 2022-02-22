using System;

namespace Manager.ManagerExceptions
{
    public class SystemErrorException : Exception // TODO Edit SystemException
    {
        public SystemErrorException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###    A SystemException occured     ###");
            Console.Error.WriteLine("  # Medium : system crashed the application");
        }

        public SystemErrorException(string message) : this()
        {
            Console.Error.Write("  # error : ");
            Console.Error.WriteLine(message);
        }

        public SystemErrorException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at : ");
            Console.Error.WriteLine(func);
        }
    }
}