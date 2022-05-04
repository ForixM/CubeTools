using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class ApplicationSettings
    {
        // Basic
        [DataMember(Name = "default")] public string? ApplicationDefault { get; set; }
        [DataMember(Name = "txt")] public string? ApplicationTxt { get; set; }
        [DataMember(Name = "pdf")] public string? ApplicationPdf { get; set; }
        // Image
        [DataMember(Name = "jpg")] public string? ApplicationJpg { get; set; }
        [DataMember(Name = "jpeg")] public string? ApplicationJpeg { get; set; }
        [DataMember(Name = "png")] public string? ApplicationPng { get; set; }
        [DataMember(Name = "ico")] public string? ApplicationIco { get; set; }
        // Others
        [DataMember(Name = "exe")] public string? ApplicationExe { get; set; }
        [DataMember(Name = "iso")] public string? ApplicationIso { get; set; }
        // Microsoft tools
        [DataMember(Name = "docx")] public string? ApplicationDocx { get; set; }
        [DataMember(Name = "odt")] public string? ApplicationOdt { get; set; }
        [DataMember(Name = "pptx")] public string? ApplicationPptx { get; set; }
        [DataMember(Name = "xlsx")] public string? ApplicationXlsx { get; set; }
        // Apple tools
        [DataMember(Name = "key")] public string? ApplicationKey { get; set; }
        // programming
        [DataMember(Name = "py")] public string? ApplicationPy { get; set; }
        [DataMember(Name = "cs")] public string? ApplicationCs { get; set; }
        [DataMember(Name = "c++")] public string? ApplicationCPlusPlus { get; set; }
        [DataMember(Name = "java")] public string? ApplicationJava { get; set; }
        [DataMember(Name = "html")] public string? ApplicationHtml { get; set; }
        [DataMember(Name = "gitignore")] public string? ApplicationGitignore { get; set; }
        // Compression
        [DataMember(Name = "zip")] public string? ApplicationZip { get; set; }
        [DataMember(Name = "rar")] public string? ApplicationRar { get; set; }
        // Music
        [DataMember(Name = "mp3")] public string? ApplicationMp3 { get; set; }
        [DataMember(Name = "alac")] public string? ApplicationAlac { get; set; }
        [DataMember(Name = "flac")] public string? ApplicationFlac { get; set; }
        [DataMember(Name = "wav")] public string? ApplicationWav { get; set; }
        // Video
        [DataMember(Name = "mp4")] public string? ApplicationMp4 { get; set; }
        [DataMember(Name = "mov")] public string? ApplicationMov { get; set; }
        [DataMember(Name = "mpeg")] public string? ApplicationMpeg { get; set; }
    }
}