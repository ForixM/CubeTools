using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class OneDriveSettings
    {
        [DataMember(Name = "account")] public OneDriveAccountSettings? Account { get; set; }
        [DataMember(Name = "registered")] public List<OneDriveAccountSettings>? Registered { get; set; }
    }

    [DataContract]
    public class OneDriveAccountSettings
    {
        [DataMember(Name = "email")] public string? Email { get; set; }
        [DataMember(Name = "password")] public string? Password { get; set; }
    }
}