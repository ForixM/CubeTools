using System;

namespace Manager.ManagerExceptions
{
    public class NoException : ManagerException
    {
        public NoException() : base()
        {
            ErrorType = "";
            CriticalLevel = "";
            Errorstd = "";
        }
    }
}