namespace Library.ManagerExceptions
{
    public class ConnectionLost : ManagerException
    {
        public ConnectionLost(string message, string func) : base("ConnectionRefusedException", Level.Info, "The connection was lost",
            message, func, true) {}
    }
}