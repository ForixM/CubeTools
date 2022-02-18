using System;

namespace Manager.ManagerExceptions
{
    public class InUseException : Exception // TODO Edit InUseException
    {
        public InUseException()
        {
            Console.Error.WriteLine("### A InUseException occured ###");
        }

        public InUseException(string message) : base()
        {
            Console.Error.Write("# output : ");
            Console.Error.WriteLine(message);
        }
    }
}