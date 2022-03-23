using System.Configuration;

namespace ConfigLoader.Sections.appInterfaceSection;

public class styleConfig : ConfigurationElement
{
    [ConfigurationProperty("name", IsRequired = true)]
    public string Name => this["name"] as string;

    [ConfigurationProperty("value", IsRequired = true)]
    public string Value => this["value"] as string;
    // Add as many ConfigurationProperty as necessary
    /*
    [ConfigurationProperty("VALUE", IsRequired = bool)]
    public string Property => this["VALUE"] as string;
     */
}