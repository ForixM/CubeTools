using System;

namespace Manager.ManagerExceptions
{
    public class AccessException : Exception // TODO Edit Access Exception
    {
        public AccessException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###      AccessException occured     ###");
            Console.Error.WriteLine("  # Low : the file/folder access is denied");
        }

        public AccessException(string message) : this()
        {
            Console.Error.Write("  # error :");
            Console.Error.WriteLine(message);
        }

        public AccessException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at :");
            Console.Error.WriteLine(func);
        }
    }
}