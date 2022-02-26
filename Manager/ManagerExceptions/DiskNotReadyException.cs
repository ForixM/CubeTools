using System;

namespace Manager.ManagerExceptions
{
    public class DiskNotReadyException : ManagerException
    {
        public DiskNotReadyException() : base()
        {
            ErrorType = "Disk is refreshing";
            CriticalLevel = "Medium";
            Errorstd = "DiskNotReadyException";
        }

        public DiskNotReadyException(string message) : base("DiskNotReadyException", "Medium", "Disk is refreshing", message)
        {
            ErrorType = "Disk is refreshing";
            CriticalLevel = "Medium";
            Errorstd = "DiskNotReadyException";
        }

        public DiskNotReadyException(string message, string func) : base("DiskNotReadyException", "Medium",
            "Disk is refreshing", message, func)
        {
            ErrorType = "Disk is refreshing";
            CriticalLevel = "Medium";
            Errorstd = "DiskNotReadyException";
        }
    }
}