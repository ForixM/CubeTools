using Avalonia.Controls;

namespace ResourcesLoader
{
    public static class ResourcesWindowIcons
    {
        public static readonly WindowIcon  MainWindowIcon = new WindowIcon(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                           "CubeToolsIconsCompressed/cubetools.ico");
    }
}