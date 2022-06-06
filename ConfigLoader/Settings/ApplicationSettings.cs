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

        public Dictionary<string, string?> Applications;

        public ApplicationSettings()
        {
            Applications = new Dictionary<string, string?>();
            
            Applications.Add("default", ApplicationDefault);
            Applications.Add("txt", ApplicationTxt);
            Applications.Add("pdf", ApplicationPdf);
            Applications.Add("jpg", ApplicationJpg);
            Applications.Add("jpeg", ApplicationJpeg);
            Applications.Add("png", ApplicationPng);
            Applications.Add("ico", ApplicationIco);
            Applications.Add("exe", ApplicationExe);
            Applications.Add("iso", ApplicationIso);
            Applications.Add("docx", ApplicationDocx);
            Applications.Add("odt", ApplicationOdt);
            Applications.Add("pptx", ApplicationPptx);
            Applications.Add("xlsx", ApplicationXlsx);
            Applications.Add("key", ApplicationKey);
            Applications.Add("py", ApplicationPy);
            Applications.Add("cs", ApplicationCs);
            Applications.Add("cpp", ApplicationCPlusPlus);
            Applications.Add("java", ApplicationJava);            
            Applications.Add("html", ApplicationHtml);
            Applications.Add("gitignore", ApplicationGitignore);
            Applications.Add("zip", ApplicationZip);
            Applications.Add("rar", ApplicationRar);
            Applications.Add("mp3", ApplicationMp3);
            Applications.Add("alac", ApplicationAlac);
            Applications.Add("flac", ApplicationFlac);
            Applications.Add("wav", ApplicationWav);
            Applications.Add("mp4", ApplicationMp4);
            Applications.Add("mov", ApplicationMov);
            Applications.Add("mpeg", ApplicationMpeg);
        }
    }
}