﻿using System;

namespace Manager.ManagerExceptions
{
    public class AccessException : ManagerException
    {
        public AccessException() : base()
        {
            ErrorType = "Data access denied";
            CriticalLevel = "Medium";
            Errorstd = "AccessException";
        }

        public AccessException(string message) : base("AccessException","Medium","Data access Denied",message)
        {
            ErrorType = "Data access denied";
            CriticalLevel = "Medium";
            Errorstd = "AccessException";
        }

        public AccessException(string message, string func) : base("AccessException","Medium","Data access Denied",message, func)
        {
            ErrorType = "Data access denied";
            CriticalLevel = "Medium";
            Errorstd = "AccessException";
        }
    }
}