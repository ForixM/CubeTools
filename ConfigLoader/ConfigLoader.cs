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
        public static string AppPath = Directory.GetCurrentDirectory().Replace('\\','/');
        
        /// <summary>
        /// Load a configuration in the ConfigSettings instance
        /// </summary>
        public static void LoadConfiguration(string name = "Config.json")
        {
            try
            {
                if (!File.Exists(name))
                {
                    File.Copy("Config.default.json", "Config.json");
                    name = "Config.json";
                }
                Settings = (ConfigSettings) JsonConvert.DeserializeObject(new StreamReader(name).ReadToEnd(), typeof(ConfigSettings))!;
                Settings.LoadedJson = AppPath + "/" + name;
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case NullReferenceException:
                        throw new Exception("Critical ERROR ! Unable to load json config files !");
                    case JsonException:
                        LogErrors.LogErrors.LogWrite("Unable to read configuration file given with path : " + name, e);
                        break;
                }
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
                File.WriteAllText(Settings.LoadedJson, JsonConvert.SerializeObject(Settings));
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case JsonException:
                        LogErrors.LogErrors.LogWrite("Unable to save the current Configuration in the config file",e);
                        break;
                    case IOException or UnauthorizedAccessException:
                        LogErrors.LogErrors.LogWrite("Unable to overwrite config files", e);
                        break;
                }
            }
        }
    }
}