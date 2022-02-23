﻿using System;

namespace Manager.ManagerExceptions
{
    public class CorruptedDirectoryException : ManagerException
    {
        public CorruptedDirectoryException() : base()
        {
            ErrorType = "Directory corrupted";
            CriticalLevel = "Critical";
            Errorstd = "CorruptedDirectoryException";
        }

        public CorruptedDirectoryException(string message) : base("CorruptedDirectoryException","Critical","Directory corrupted",message)
        {
            ErrorType = "Directory corrupted";
            CriticalLevel = "Critical";
            Errorstd = "CorruptedDirectoryException";
        }

        public CorruptedDirectoryException(string message, string func) : base("CorruptedDirectoryException","Critical","Directory corrupted",message, func)
        {
            ErrorType = "Directory corrupted";
            CriticalLevel = "Critical";
            Errorstd = "CorruptedDirectoryException";
        }
    }
}