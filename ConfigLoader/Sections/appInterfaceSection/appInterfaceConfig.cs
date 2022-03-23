using System.Configuration;

namespace ConfigLoader.Sections.appInterfaceSection;

public class appInterfaceConfig : ConfigurationSection
{
    
    public static appInterfaceConfig GetConfig() => (appInterfaceConfig) ConfigurationManager.GetSection("appInterface") ?? new appInterfaceConfig();
    
    // Sub Collections
    // Style :
    [ConfigurationProperty("Styles")]
    [ConfigurationCollection(typeof(appInterfaceConfig), AddItemName = "Style")]
    public stylesConfig Styles
    {
        get
        {
            var o = this["Styles"];
            return o as stylesConfig;
        }
    }
    
    // Geometrics :
    [ConfigurationProperty("Geometrics")]
    [ConfigurationCollection(typeof(appInterfaceConfig), AddItemName = "Geometric")]
    public geometricsConfig Geometrics
    {
        get
        {
            var o = this["Geometrics"];
            return o as geometricsConfig;
        }
    }
}