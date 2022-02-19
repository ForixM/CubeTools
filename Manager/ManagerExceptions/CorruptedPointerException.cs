using System;

namespace Manager.ManagerExceptions
{
    public class CorruptedPointerException :  Exception
    {
        public CorruptedPointerException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###   An CorruptedPointer occured    ###");
            Console.Error.WriteLine("  # Medium : You should restart CubeTools or Refresh files ###");
        }

        public CorruptedPointerException(string message) : this()
        {
            Console.Error.Write("# error :");
            Console.Error.WriteLine(message);
        }

        public CorruptedPointerException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at :");
            Console.Error.WriteLine(func);
        }
    }
}