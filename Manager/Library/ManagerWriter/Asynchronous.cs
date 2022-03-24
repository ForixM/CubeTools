using System.Security;
using Library.ManagerExceptions;

namespace Library.ManagerWriter;

public partial class ManagerWriter
{
    public static async Task CopyAsync(string source, string dest)
    {
        await Task.Run( () => Copy(source, dest, true));
    }
    
}