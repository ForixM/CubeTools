using System.Configuration;

namespace ConfigLoader;

public static partial class ConfigLoader
{
    // This region implements all methods used to write inside the .config files
    //

    public static void ModifyParameters()
    {
        
    }

    public static void ModifyParameters(ConfigurationSection cs)
    {
        switch (cs.ToString())
        {
            case "AppLauncher":
                
                break;
            default :
                break;
        }
    }

    public static void AddParameters()
    {
        
    }

    public static void AddParameters(string section)
    {
        
    }

    public static void AddSection(string section)
    {
        
    }
}