using System.Configuration;

namespace ConfigLoader
{
    
    public static partial class ConfigLoader
    {
        // This region implements all methods used to write inside the .config files
        //

        public static void ModifyParameters(selectedSection section)
        {
        }

        public static void ModifyParameters(ConfigurationSection cs)
        {
            switch (cs.ToString())
            {
                case "AppLauncher":

                    break;
            }
        }

        public static void AddParameters()
        {
        }

        public static void AddParameters(ConfigurationSection section)
        {
        
        }

        public static void AddSection(string section)
        {
        }
    }
}