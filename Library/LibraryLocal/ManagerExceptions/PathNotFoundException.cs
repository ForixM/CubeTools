namespace Library.ManagerExceptions
{
    public class PathNotFoundException : ManagerException
    {
        public PathNotFoundException(string message, string func) : base("PathNotFoundException", Level.Info,
            "Path does not exist", message, func, true) { }
    }
}