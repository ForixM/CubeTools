using System.Configuration;

namespace ConfigLoader.Sections.appLauncherSection;

public class appLauncherConfig : ConfigurationSection
{
    [ConfigurationProperty("Applications")]
    [ConfigurationCollection(typeof(applicationsConfig), AddItemName = "Application")]
    public applicationsConfig Applications
    {
        get
        {
            var o = this["Applications"];
            return o as applicationsConfig;
        }
    }

    public static appLauncherConfig GetConfig()
    {
        return (appLauncherConfig) ConfigurationManager.GetSection("appLauncher") ?? new appLauncherConfig();
    }
}