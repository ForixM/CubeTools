﻿namespace Library.ManagerExceptions
{
    public class ReplaceException : ManagerException
    {
        public ReplaceException()
        {
            ErrorType = "Replace data failed";
            CriticalLevel = "Medium";
            Errorstd = "ReplaceException";
        }

        public ReplaceException(string message) : base("ReplaceException", "Medium", "Replace data failed", message)
        {
            ErrorType = "Replace data failed";
            CriticalLevel = "Medium";
            Errorstd = "ReplaceException";
        }

        public ReplaceException(string message, string func) : base("ReplaceException", "Medium", "Replace data failed",
            message, func)
        {
            ErrorType = "Replace data failed";
            CriticalLevel = "Medium";
            Errorstd = "ReplaceException";
        }
    }
}