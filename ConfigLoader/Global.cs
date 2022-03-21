using System.Collections.Specialized;
using System.Configuration;
using ConfigLoader.Sections.appLauncherSection;

namespace ConfigLoader;

/// <summary>
/// - Purpose : Implements all needed functions to read properties for runtime and UI implementation of the software <br></br> // TODO Edit description
/// </summary>
public static partial class ConfigLoader
{
    public static string AppPathSettings = ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.None ).FilePath;
    public static NameValueCollection AppSettings = ConfigurationManager.AppSettings;
    // Add sections
    public static appLauncherConfig AppLauncher = appLauncherConfig.GetConfig();
}