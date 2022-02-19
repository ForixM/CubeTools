using System;

namespace Manager.ManagerExceptions
{
    public class SystemException : Exception // TODO Edit SystemException
    {
        public SystemException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###    A SystemException occured     ###");
            Console.Error.WriteLine("  # Medium : system crashed the application");
        }

        public SystemException(string message) : this()
        {
            Console.Error.Write("  # error : ");
            Console.Error.WriteLine(message);
        }

        public SystemException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at : ");
            Console.Error.WriteLine(func);
        }
    }
}