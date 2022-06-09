using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class StylesSettings
    {
        [DataMember(Name = "theme")] public string Theme { get; set; }
        [DataMember(Name = "pack")] public string Pack { get; set; }

        public StylesSettings()
        {
            Theme = "Light";
            Pack = "Assets/Default - Light";
        }
    }
}