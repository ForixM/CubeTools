using System.Configuration;

namespace ConfigLoader.Sections.appLauncherSection;

public class appLauncherConfig : ConfigurationSection
{
    public static appLauncherConfig GetConfig()
    {
        return (appLauncherConfig)System.Configuration.ConfigurationManager.GetSection("appLauncher") ?? new appLauncherConfig();
    }

    [ConfigurationProperty("Applications")]
    [ConfigurationCollection(typeof(applicationsConfig), AddItemName = "Application")]
    public applicationsConfig Applications
    {
        get
        {
            object o = this["Applications"];
            return o as applicationsConfig ;
        }
    }
}