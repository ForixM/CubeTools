using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ConfigLoader.Settings
{
    public class AssetsResource
    {
        public static IImage CreateIcon =>
            new Bitmap(ConfigLoader.Settings.AppPath + "/" + ConfigLoader.Settings.Styles!.Pack + "/CubeToolsIconsCompressed/Create.ico");

    }
}