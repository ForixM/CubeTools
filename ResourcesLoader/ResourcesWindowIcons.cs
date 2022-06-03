using Avalonia.Controls;

namespace ResourcesLoader
{
    public static class ResourcesWindowIcons
    {
        public static readonly WindowIcon  MainWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/MainWindow.ico");

        public static readonly WindowIcon CreateFileWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/CreateFileWindow.ico");
        
        public static readonly WindowIcon CreateFolderWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/CreateFolderWindow.ico");
        
        public static readonly WindowIcon DeleteWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/DeleteWindow.ico");
        
        public static readonly WindowIcon RenameWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/RenameWindow.ico");
        
        public static readonly WindowIcon SearchWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/SearchWindow.ico");
        
        public static readonly WindowIcon SortWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/SortWindow.ico");
        
        public static readonly WindowIcon SettingsWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/SettingsWindow.ico");
        
        public static readonly WindowIcon PropertiesWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/PropertiesWindow.ico");

        public static readonly WindowIcon CompressWindowIcon = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/CompressWindow.ico");
        
        public static readonly WindowIcon ErrorWindow = new WindowIcon(
            ConfigLoader.ConfigLoader.Settings.ResourcePath +
            "Window/ErrorWindow.ico");
    }
}