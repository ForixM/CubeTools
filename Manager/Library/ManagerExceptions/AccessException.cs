namespace Library.ManagerExceptions
{
    public class AccessException : ManagerException
    {
        public AccessException(string message, string func) : base("AccessException", Level.High, "Data access Denied",
            message, func, true) {}
    }
}