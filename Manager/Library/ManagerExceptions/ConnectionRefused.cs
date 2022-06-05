namespace Library.ManagerExceptions
{
    public class ConnectionRefused : ManagerException
    {
        public ConnectionRefused(string message, string func) : base("ConnectionRefusedException", Level.Normal, "The connection was refused",
            message, func, true) {}
    }
}