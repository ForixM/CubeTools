namespace Library.ManagerExceptions
{
    public class CorruptedPointerException : ManagerException
    {
        public CorruptedPointerException(string message, string func) : base("CorruptedPointerException", Level.High,
            "Pointer corrupted", message, func, true) { }
    }
}