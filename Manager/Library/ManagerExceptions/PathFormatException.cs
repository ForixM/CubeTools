namespace Library.ManagerExceptions
{
    public class PathFormatException : ManagerException
    {
        public PathFormatException(string message, string func) : base("PathFormatException", Level.Normal,
            "Format of the path is incorrect", message, func) { }
    }
}