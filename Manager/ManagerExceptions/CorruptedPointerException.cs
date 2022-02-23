using System;

namespace Manager.ManagerExceptions
{
    public class CorruptedPointerException :  ManagerException
    {
        public CorruptedPointerException() : base()
        {
            ErrorType = "Pointer corrupted";
            CriticalLevel = "High";
            Errorstd = "CorruptedPointerException";
        }

        public CorruptedPointerException(string message) : base("CorruptedPointerException","High","Pointer corrupted",message)
        {
            ErrorType = "Pointer corrupted";
            CriticalLevel = "High";
            Errorstd = "CorruptedPointerException";
        }

        public CorruptedPointerException(string message, string func) : base("CorruptedPointerException","High","Pointer corrupted",message, func)
        {
            ErrorType = "Pointer corrupted";
            CriticalLevel = "High";
            Errorstd = "CorruptedPointerException";
        }
    }
}