﻿using System;

namespace Manager.ManagerExceptions
{
    public class UnknownException : Exception
    {
        public UnknownException()
        {
            Console.Error.WriteLine("########################################");
            Console.Error.WriteLine("###     An unknown error occured     ###");
            Console.Error.WriteLine("  # Critical : Consider rebooting  the application");
        }

        public UnknownException(string message) : this()
        {
            Console.Error.Write("  # error : ");
            Console.Error.WriteLine(message);
        }

        public UnknownException(string message, string func) : this(message)
        {
            Console.Error.Write("  # error at : ");
            Console.Error.WriteLine(func);
        }
    }
}