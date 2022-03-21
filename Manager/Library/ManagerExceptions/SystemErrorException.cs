namespace Library.ManagerExceptions
{
    public class SystemErrorException : ManagerException
    {
        public SystemErrorException()
        {
            ErrorType = "System crashed the app";
            CriticalLevel = "Medium";
            Errorstd = "SystemErrorException";
        }

        public SystemErrorException(string message) : base("SystemErrorException", "Medium", "System crashed the app",
            message)
        {
            ErrorType = "System crashed the app";
            CriticalLevel = "Medium";
            Errorstd = "SystemErrorException";
        }

        public SystemErrorException(string message, string func) : base("SystemErrorException", "Medium",
            "System crashed the app", message, func)
        {
            ErrorType = "System crashed the app";
            CriticalLevel = "Medium";
            Errorstd = "SystemErrorException";
        }
    }
}