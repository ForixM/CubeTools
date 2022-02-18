using System;

namespace Manager.ManagerExceptions
{
    public class UnknownException : Exception
    {
        public UnknownException()
        {
            Console.Error.WriteLine("### An unknown error occured ###");
            Console.Error.WriteLine("### Consider reboot the application ###");
        }

        public UnknownException(string message) : base()
        {
            Console.Error.Write("# error : ");
            Console.Error.WriteLine(message);
        }
    }
}