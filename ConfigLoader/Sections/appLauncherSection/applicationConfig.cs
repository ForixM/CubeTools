using System.Configuration;

namespace ConfigLoader.Sections.appLauncherSection;

public class applicationConfig : ConfigurationElement
{
    [ConfigurationProperty("extension", IsRequired = true)]
    public string Name
    {
        get
        {
            return this["extension"] as string;
        }
    }
    [ConfigurationProperty("app", IsRequired = true)]
    public string Value
    {
        get
        {
            return this["app"] as string;
        }
    }
}