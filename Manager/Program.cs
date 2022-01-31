using Reader = Manager.ManagerReader;
using Writer = Manager.ManagerWriter;
using System;
using System.Security.Principal;
using System.Linq;
using System.Security.AccessControl;

namespace Manager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DirectoryType directoryType = new DirectoryType("C:/Users/mateo/OneDrive/Documents/Professional");
            foreach (var ft in Reader.SortByModifiedDate(directoryType.ChildrenFiles))
                Console.WriteLine(ft.Date);
        }
    }
}
