using System;

namespace Manager.ManagerExceptions
{
    public class AccessException : Exception // TODO Edit Access Exception
    {
        public AccessException()
        {
            Console.Error.WriteLine("### An AccessException occured ###");
        }

        public AccessException(string message) : base()
        {
            Console.Error.Write("# output : ");
            Console.Error.WriteLine(message);
        }
    }
}