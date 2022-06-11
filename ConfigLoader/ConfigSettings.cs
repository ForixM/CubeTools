using System.Runtime.Serialization;
using Avalonia.Input;
using ConfigLoader.Settings;

namespace ConfigLoader
{
    [DataContract]
    public class ConfigSettings
    {
        // Global
        public string AppPath { get; set; }
        /// <summary>
        /// Application Settings : All settings related to the way other program will be executed --
        /// "default" "txt"
        /// "pdf" "jpg" "jpeg" "png" "ico" "exe" "iso" "docx"
        /// "odt" "pptx" "xlsx" "key" "py" "cs" "cpp" "java" "html" "gitignore" "zip" "rar" "mp3"
        ///"alac" "flac" "wav" "mp4""mov" "mpeg"
        /// </summary>
        [DataMember(Name = "Application")] public Dictionary<string, string>? Application { get; set; }
        /// <summary>
        /// Styles Settings : All settings related to the style of the software
        /// </summary>
        [DataMember(Name="Styles")] public StylesSettings? Styles { get; set; }
        /// <summary>
        /// FTP Settings : Allow the user to load its own configuration
        /// </summary>
        [DataMember(Name = "FTP")] public FtpSettings? Ftp { get; set; }
        /// <summary>
        /// Shortcuts Settings : Allow the user to custom its shortcuts <br></br>
        /// "copy" <br></br>
        /// "cut" <br></br>
        /// "close" <br></br>
        /// "paste" <br></br>
        /// "newWindow" <br></br>
        /// "search" <br></br>
        /// "delete" <br></br>
        /// "deletePermanent" <br></br>
        /// "selectAll" <br></br>
        /// "createFile" <br></br>
        /// "createDir" <br></br>
        /// "rename" <br></br>
        /// "settings" <br></br>
        /// "reload" <br></br>
        /// </summary>
        [DataMember(Name = "Shortcuts")] public Dictionary<string, List<Key>>? Shortcuts { get; set; }
        public readonly List<List<Key>> ListShortcuts;
        /// <summary>
        /// Links Settings : Allow the user to have its own favorite links
        /// </summary>
        [DataMember(Name = "Links")] public Dictionary<string, string>? Links { get; set; }

        public string ResourcePath => $"{AppPath}/Assets/{(Styles.IsLight ? Styles.FolderLight : Styles.FolderDark)}/";
        public string LoadedJson;
        public ConfigSettings()
        {
            AppPath = Directory.GetCurrentDirectory().Replace('\\', '/');
            LoadedJson = AppPath + "/" + "Config.json";
            ListShortcuts = new List<List<Key>>();
        }
    }
}