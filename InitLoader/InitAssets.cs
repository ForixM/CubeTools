using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;

namespace InitLoader
{
    /// <summary>
    /// This class will help to initialize directories and path needed for the application so that it can run correctly
    /// </summary>
    public static partial class InitLoader
    {
        private static void InitAssets()
        {
            // Get pack of assets
            if (!Directory.Exists("Assets") || !Directory.Exists("Assets/Default - Light"))
                throw new ManagerException("Critical Error while loading assets", Level.Crash, "Assets not found","Assets could not be find in the given env");
            if (ConfigLoader.ConfigLoader.Settings.AssetsPath != "Assets/Default - Light")
            {
                if (Directory.Exists("Default - Light") && Directory.EnumerateDirectories("Assets").Count() >= 2 && ConfigLoader.ConfigLoader.Settings.AssetsPath != null)
                    _getPackagingAssets(ConfigLoader.ConfigLoader.Settings.AssetsPath, "Assets/Default - Light");
                else
                    ConfigLoader.ConfigLoader.Settings.AssetsPath = "Assets/Default - Light";
            }
            
            // 
        }

        /// <summary>
        /// Load assets of the application
        /// </summary>
        /// <param name="packPath">The current path of the pack</param>
        /// <param name="defaultPath">The default path of the default pack</param>
        private static void _getPackagingAssets(string packPath, string defaultPath)
        {
            if (!Directory.Exists(defaultPath)) return;
            
            // 1) Verifying files
            var filesPack = Directory.EnumerateFiles(packPath);
            var filesDefault = Directory.EnumerateFiles(defaultPath);
            foreach (var file in filesDefault)
            {
                string dest = packPath + "/" + ManagerReader.GetPathToName(file);;
                if (!filesPack.Contains(dest)) File.Copy(file, dest);
            }
            // 2) Browsing directories
            var dirsPack = Directory.EnumerateDirectories(packPath);
            var dirsDefault = Directory.EnumerateDirectories(packPath);
            foreach (var dir in dirsDefault)
            {
                string dest = packPath + "/" + ManagerReader.GetPathToName(dir);
                if (dirsPack.Contains(dest)) _getPackagingAssets(dest, dir);
                else ManagerWriter.CopyDirectory(dir, dest, true);
            }
        }
    }
}