using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ConfigLoader
{
    public static class ConfigLoader
    {
        /// <summary>
        /// All Settings wrapping up in one single variable : FANTASTIC
        /// </summary>
        public static ConfigSettings Settings;
        
        
        /// <summary>
        /// Load a configuration in the ConfigSettings instance
        /// </summary>
        public static void LoadConfiguration(string path = "Config.json")
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Copy("Config.default.json", "Config.json");
                    path = "Config.json";
                }
                string content;
                using (StreamReader reader = new StreamReader(path))
                {
                    content = reader.ReadToEnd();
                }

                Settings = (ConfigSettings) JsonConvert.DeserializeObject(content, typeof(ConfigSettings));
                Settings.LoadedJson = Path.GetFileName(path);
            }
            catch (Exception e)
            {
                if (e is JsonException)
                    LogErrors.LogErrors.LogWrite("Unable to read configuration file given with path : " + path, e);
            }

            if (Settings == null)
                throw new Exception("Critical ERROR ! Unable to load json config files !");
        }
        /// <summary>
        /// Save the instance ConfigSettings in the json configuration file
        /// </summary>
        public static void SaveConfiguration(string path = "Config.json")
        {
            try
            {
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception) { }
                }
                File.Create(JsonSerializer.Serialize(Settings));
            }
            catch (Exception e)
            {
                if (e is JsonException)
                    LogErrors.LogErrors.LogWrite("Unable to save the current Configuration in the config file",e);
                else if (e is IOException or UnauthorizedAccessException)
                    LogErrors.LogErrors.LogWrite("Unable tod estroy config files", e);
            }
        }
    }
}