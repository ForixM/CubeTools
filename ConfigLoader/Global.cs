using System.Collections.Specialized;
using System.Configuration;
using ConfigLoader.Sections.appInterfaceSection;
using ConfigLoader.Sections.appLauncherSection;

namespace ConfigLoader;

/// <summary>
///     - Purpose : Implements all needed functions to read properties for runtime and CubeTools UI implementation of the
///     software <br></br> // TODO Edit description
/// </summary>
public static partial class ConfigLoader
{
    // The global path of the config file
    public static string AppPathSettings =
        ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
    
    // General values
    public static NameValueCollection AppSettings = ConfigurationManager.AppSettings;
    
    // CustomSection List
    public static List<ConfigurationSection> cs = new List<ConfigurationSection>() {AppLauncher, AppInterface};
    // Add sections
    public static appLauncherConfig AppLauncher = appLauncherConfig.GetConfig();
    public static appInterfaceConfig AppInterface = appInterfaceConfig.GetConfig();
    
    // Section Enum for easiest usage in code
    public enum selectedSection
    {
        Default,
        AppSettingsSection, // appSettings
        AppLauncherSection, // appLauncher 
        AppInterfaceSection, // appInterface
    }
    // Static enum for easiest usage in code
    public enum selectedCollection : int
    {
        None,
        // appLauncher
        ApplicationsCollection,
        
        // appInterface
        StyleCollection,
        GeometricsCollection
        
        // Add as many as you want
    }
}
