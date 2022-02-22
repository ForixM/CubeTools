﻿using System;

namespace Manager.ManagerExceptions
{
    public class CorruptedDirectoryException : Exception
    {
        public CorruptedDirectoryException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###A CorruptedDirectoryException occured###");
            Console.Error.WriteLine("  # High : the current loaded directory is corrupted");
        }

        public CorruptedDirectoryException(string message) : this()
        {
            Console.Error.Write("  # error :");
            Console.Error.WriteLine(message);
        }

        public CorruptedDirectoryException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error :");
            Console.Error.WriteLine(func);
        }
    }
}