using System;

namespace Manager.ManagerExceptions
{
    public class PathFormatException : ManagerException
    {
        public PathFormatException() : base()
        {
            ErrorType = "Format of the path is incorrect";
            CriticalLevel = "Medium";
            Errorstd = "PathFormatException";
        }

        public PathFormatException(string message) : base("PathFormatException", "Medium", "Format of the path is incorrect", message)
        {
            ErrorType = "Format of the path is incorrect";
            CriticalLevel = "Medium";
            Errorstd = "DiskNotReadyException";
        }

        public PathFormatException(string message, string func) : base("PathFormatException", "Medium",
            "Format of the path is incorrect", message, func)
        {
            ErrorType = "Format of the path is incorrect";
            CriticalLevel = "Medium";
            Errorstd = "PathFormatException";
        }
    }
}