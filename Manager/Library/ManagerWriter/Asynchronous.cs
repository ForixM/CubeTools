using System.Security;
using System.Threading.Tasks;
using Library.ManagerExceptions;
using Library.Pointers;

namespace Library.ManagerWriter
{
    public partial class ManagerWriter
    {
        public static async Task CopyAsync(string source, string dest)
        {
            await Task.Run( () => Copy(source, dest, true));
        }

        public static async Task<FileType> CopyAsync(FileType ft)
        {
            return await Task.Run(() => Copy(ft.Path));
        }
    
    }
}

