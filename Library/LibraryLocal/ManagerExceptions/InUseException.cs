namespace Library.ManagerExceptions
{
    public class InUseException : ManagerException
    {
        public InUseException(string message, string func) : base("InUseException", Level.Info, "Data already used",
            message, func, true) {}
    }
}