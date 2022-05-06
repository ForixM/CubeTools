using System.Runtime.Serialization;
using ConfigLoader.Settings;

namespace ConfigLoader
{
    [DataContract]
    public class ConfigSettings
    {
        // Global
        [DataMember(Name="AssetsPath")] public string? AssetsPath { get; set; }
        public string AppPath { get; set; }
        // Sections
        [DataMember(Name = "Application")] public ApplicationSettings? Application { get; set; }
        [DataMember(Name="Styles")] public StylesSettings? Styles { get; set; }
        [DataMember(Name = "OneDrive")] public OneDriveSettings? OneDrive { get; set; }
        [DataMember(Name = "GoogleDrive")] public GoogleDriveSettings? GoogleDrive { get; set; }
        [DataMember(Name = "FTP")] public FtpSettings? Ftp { get; set; }
        [DataMember(Name = "Shortcuts")] public ShortcutsSettings? Shortcuts { get; set; }

        public string ResourcePath => AppPath + "/" + (Styles != null ? Styles.Pack : "Assets/default") + "/";

        public ConfigSettings()
        {
            AppPath = Directory.GetCurrentDirectory();
        }
    }
}