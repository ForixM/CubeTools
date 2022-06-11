using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class StylesSettings
    {
        [DataMember(Name = "folder_light")] public string? FolderLight { get; set; }
        [DataMember(Name = "folder_dark")] public string? FolderDark { get; set; }
        [DataMember(Name = "is_light")] public bool IsLight { get; set; }
    }
}