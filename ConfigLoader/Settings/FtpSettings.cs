using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class FtpSettings
    {
        [DataMember(Name = "servers")] private List<FtpServersSettings>? Servers { get; set; }
    }

    public class FtpServersSettings
    {
        [DataMember(Name = "host")] public string? Host { get; set; }
        [DataMember(Name = "login")] public string? Login { get; set; }
        [DataMember(Name = "password")] public string? Password { get; set; }
        [DataMember(Name = "port")] public string? Port { get; set; }
    }
}