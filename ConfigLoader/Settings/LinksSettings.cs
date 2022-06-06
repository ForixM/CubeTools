using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class LinksSettings
    {
        [DataMember(Name = "links")] public List<LinkSettings> Links { get; set; }

        public LinksSettings()
        {
            Links = new List<LinkSettings>();
        }
    }

    [DataContract]
    public class LinkSettings
    {
        [DataMember(Name = "path")] public string? Path;
        [DataMember(Name = "name")] public string? Name;
    }
}