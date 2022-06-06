namespace InitLoader
{
    /// <summary>
    /// This class will help to initialize directories and path needed for the application so that it can run correctly
    /// </summary>
    public static partial class InitLoader
    {
        private static void InitConfig()
        {
            // Try to find a json config file : default one is Config.json
            if (File.Exists("Config.json"))
                ConfigLoader.ConfigLoader.LoadConfiguration();
            else if (File.Exists("Config.default.json"))
                ConfigLoader.ConfigLoader.LoadConfiguration("Config.default.json");
            else
            {
                // Try to search a json file starting with 'Config'
                string? path;
                try
                {
                    path = Directory.GetCurrentDirectory();
                    foreach (var file in Directory.EnumerateFiles(path))
                    {
                        if (file.StartsWith("Config"))
                        {
                            ConfigLoader.ConfigLoader.LoadConfiguration(file);
                            ConfigLoader.ConfigLoader.Settings.AppPath = Directory.GetCurrentDirectory().Replace('\\','/');
                            return;
                        }
                    }
                }
                catch (Exception e)
                {
                    LogErrors.LogErrors.LogWrite("Unable to find json file for configuration", e);
                }

                // Create manually a default one
                var stream = File.Create("Config.default.json");
                StreamWriter sr = new StreamWriter(stream);
                const string text = "{" +
                                    "\"AssetsPath\" : \"Assets\"," +
                                    "\"AppPath\" : \".\"," +
                                    "\"Styles\" : { \"themes\" : \"default\" , \"pack\" : \"Assets/default\"}," +
                                    "\"FTP\" : { \"servers\" : [] }," +
                                    "\"Shortcuts\" :{ }," +
                                    "\"Application\" :{}" +
                                    "\"Links\" : { links : [] }" +
                                    "}";
                sr.Write(text);
                sr.Close();
                // Generate configuration
                ConfigLoader.ConfigLoader.LoadConfiguration("Config.default.json");
            }
        }
    }
}