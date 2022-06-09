using Newtonsoft.Json;
using JsonException = System.Text.Json.JsonException;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ConfigLoader
{
    public static class ConfigLoader
    {
        /// <summary>
        /// All Settings wrapping up in one single variable
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
                Settings = (ConfigSettings) JsonConvert.DeserializeObject(new StreamReader(path).ReadToEnd(), typeof(ConfigSettings))!;
                Settings.AppPath = Directory.GetCurrentDirectory().Replace('\\','/');
                Settings.LoadedJson = Path.Combine(Settings.AppPath, Path.GetFileName(path));
            }
            catch (Exception e)
            {
                if (e is NullReferenceException) throw new Exception("Critical ERROR ! Unable to load json config files !");
                if (e is JsonException) LogErrors.LogErrors.LogWrite("Unable to read configuration file given with path : " + path, e);
            }

            
        }
        /// <summary>
        /// Save the instance ConfigSettings in the json configuration file
        /// </summary>
        public static void SaveConfiguration()
        {
            try
            {
                File.Delete(Settings.LoadedJson);
                var temp = JsonConvert.SerializeObject(Settings);
                // File.CreateText(path).WriteLine(JsonConvert.SerializeObject(Settings));
                File.WriteAllText(Settings.LoadedJson, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception e)
            {
                if (e is JsonException)
                    LogErrors.LogErrors.LogWrite("Unable to save the current Configuration in the config file",e);
                else if (e is IOException or UnauthorizedAccessException)
                    LogErrors.LogErrors.LogWrite("Unable to overwrite config files", e);
            }
        }
    }
}