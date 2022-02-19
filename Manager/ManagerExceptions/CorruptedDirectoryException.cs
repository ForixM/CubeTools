using System;

namespace Manager.ManagerExceptions
{
    public class CorruptedDirectoryException : Exception
    {
        public CorruptedDirectoryException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("### An CorruptedDirectory occured ###");
            Console.Error.WriteLine("  # Critical : You should restart CubeTools ###");
        }

        public CorruptedDirectoryException(string message) : this()
        {
            Console.Error.Write("# error : ");
            Console.Error.WriteLine(message);
        }

        public CorruptedDirectoryException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at : ");
            Console.Error.WriteLine(func);
        }
    }
}