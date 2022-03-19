using System.IO;

namespace Manager
{
    public static class RunCLI
    {
        private static void Init()
        {
            // Add Code here to initialize CLI
        }
        public static void Launch()
        {
            CLI cl = new CLI(Directory.GetCurrentDirectory());
            cl.Process();
        }
    }
}