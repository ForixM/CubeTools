namespace Library.ManagerExceptions
{
    public class ReplaceException : ManagerException
    {
        public ReplaceException(string message, string func) : base("ReplaceException", Level.High, "Replace data failed",
            message, func) { }
    }
}