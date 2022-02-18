using System;

namespace Manager.ManagerExceptions
{
    public class SystemException : Exception // TODO Edit SystemException
    {
        public SystemException()
        {
            Console.Error.WriteLine("### A SystemException occured ###");
        }

        public SystemException(string message) : base()
        {
            Console.Error.Write("# output : ");
            Console.Error.WriteLine(message);
        }
    }
}