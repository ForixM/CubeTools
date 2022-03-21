namespace Library.ManagerExceptions
{
    public class NoException : ManagerException
    {
        public NoException()
        {
            ErrorType = "";
            CriticalLevel = "";
            Errorstd = "";
        }
    }
}