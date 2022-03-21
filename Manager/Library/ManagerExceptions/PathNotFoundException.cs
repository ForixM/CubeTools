namespace Library.ManagerExceptions
{
    public class PathNotFoundException : ManagerException
    {
        public PathNotFoundException()
        {
            ErrorType = "Path does not exist";
            CriticalLevel = "Low";
            Errorstd = "PathNotFoundException";
        }

        public PathNotFoundException(string message) : base("PathNotFoundException", "Low", "Path does not exist",
            message)
        {
            ErrorType = "Path does not exist";
            CriticalLevel = "Low";
            Errorstd = "PathNotFoundException";
        }

        public PathNotFoundException(string message, string func) : base("PathNotFoundException", "Low",
            "Path does not exist", message, func)
        {
            ErrorType = "Path does not exist";
            CriticalLevel = "Low";
            Errorstd = "PathNotFoundException";
        }
    }
}