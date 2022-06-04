namespace Library.ManagerExceptions
{
    public class DiskNotReadyException : ManagerException
    {
        public DiskNotReadyException(string message, string func) : base("DiskNotReadyException", Level.Info,
            "Disk is refreshing", message, func, true) { }
    }
}