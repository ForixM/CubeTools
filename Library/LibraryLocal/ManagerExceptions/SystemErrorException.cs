namespace Library.ManagerExceptions
{
    public class SystemErrorException : ManagerException
    {
        public SystemErrorException(string message, string func) : base("SystemErrorException", Level.Crash,
            "System crashed the app", message, func, true) { }
    }
}