namespace Library.ManagerExceptions
{
    public class CorruptedDirectoryException : ManagerException
    {
        public CorruptedDirectoryException(string message, string func) : base("CorruptedDirectoryException",
            Level.Crash, "Directory corrupted", message, func, true) { }
    }
}