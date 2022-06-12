using Avalonia.Controls;

namespace ResourcesLoader
{
    public static class ResourcesWindowIcons
    {
        public static WindowIcon  MainWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/MainWindow.ico");

        public static WindowIcon CreateFileWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/CreateFileWindow.ico");
        
        public static WindowIcon CreateFolderWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/CreateFolderWindow.ico");
        
        public static WindowIcon DeleteWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/DeleteWindow.ico");
        
        public static WindowIcon RenameWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/RenameWindow.ico");
        
        public static WindowIcon SearchWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/SearchWindow.ico");
        
        public static WindowIcon SortWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/SortWindow.ico");
        
        public static WindowIcon SettingsWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/SettingsWindow.ico");
        
        public static WindowIcon PropertiesWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/PropertiesWindow.ico");

        public static WindowIcon CompressWindowIcon => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/CompressWindow.ico");
        
        public static WindowIcon ErrorWindow => new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/ErrorWindow.ico");

        public static WindowIcon FtpWindowIcon => new WindowIcon(
                        ConfigLoader.ConfigLoader.Settings.ResourcePath +
                        "IconsCompressed/FTP.ico");
    }
}