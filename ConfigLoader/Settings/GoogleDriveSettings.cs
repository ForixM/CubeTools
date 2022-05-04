using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class GoogleDriveSettings
    {
        [DataMember(Name = "account")] public GoogleDriveAccountSettings? Account { get; set; }
        [DataMember(Name = "registered")] public List<GoogleDriveAccountSettings>? Registered { get; set; }
        
    }

    [DataContract]
    public class GoogleDriveAccountSettings
    {
        [DataMember(Name = "email")] public string? Email { get; set; }
        [DataMember(Name = "password")] public string? Password { get; set; }
    }
}