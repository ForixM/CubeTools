namespace Library.ManagerExceptions
{
    public class PathFormatException : ManagerException
    {
        public PathFormatException()
        {
            ErrorType = "Format of the path is incorrect";
            CriticalLevel = "Medium";
            Errorstd = "PathFormatException";
        }

        public PathFormatException(string message) : base("PathFormatException", "Medium",
            "Format of the path is incorrect", message)
        {
            ErrorType = "Format of the path is incorrect";
            CriticalLevel = "Medium";
            Errorstd = "PathFormatException";
        }

        public PathFormatException(string message, string func) : base("PathFormatException", "Medium",
            "Format of the path is incorrect", message, func)
        {
            ErrorType = "Format of the path is incorrect";
            CriticalLevel = "Medium";
            Errorstd = "PathFormatException";
        }
    }
}