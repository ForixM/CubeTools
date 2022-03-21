namespace Library.ManagerExceptions
{
    public class InUseException : ManagerException
    {
        public InUseException()
        {
            ErrorType = "Data already used";
            CriticalLevel = "Medium";
            Errorstd = "InUseException";
        }

        public InUseException(string message) : base("InUseException", "Medium", "Data already used", message)
        {
            ErrorType = "Data already used";
            CriticalLevel = "Medium";
            Errorstd = "InUseException";
        }

        public InUseException(string message, string func) : base("InUseException", "Medium", "Data already used",
            message, func)
        {
            ErrorType = "Data already used";
            CriticalLevel = "Medium";
            Errorstd = "InUseException";
        }
    }
}