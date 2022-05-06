using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ResourcesLoader
{
    public static class ResourcesExtensions
    {

        public static readonly IImage ImageExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "CubeToolsAppsExtensionsCompressed/jpg.ico");

        public static readonly IImage TextExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "CubeToolsAppsExtensionsCompressed/pages.ico");

        public static readonly IImage ExeExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "CubeToolsAppsExtensionsCompressed/exe.ico");

        public static readonly IImage DocxExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "CubeToolsAppsExtensionsCompressed/docx.ico");

        public static readonly IImage PdfExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "CubeToolsAppsExtensionsCompressed/pdf.ico");

        public static readonly IImage PptxExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "CubeToolsAppsExtensionsCompressed/pptx.ico");

        public static readonly IImage PyExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "CubeToolsAppsExtensionsCompressed/py.ico");

        public static readonly IImage CsExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "CubeToolsAppsExtensionsCompressed/csharp.ico");

        public static readonly IImage CPlusPlusExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "CubeToolsAppsExtensionsCompressed/c++.ico");

        public static readonly IImage JavaExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "CubeToolsAppsExtensionsCompressed/java.ico");

        public static readonly IImage HtmlExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "CubeToolsAppsExtensionsCompressed/html.ico");

        public static readonly IImage XslsExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "CubeToolsAppsExtensionsCompressed/xsls.ico");

        public static readonly IImage GitIgnoreExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "CubeToolsAppsExtensionsCompressed/gitignore.ico");

        public static readonly IImage CompressExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                     "CubeToolsAppsExtensionsCompressed/zip.ico");

        public static readonly IImage ArchiveExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "CubeToolsAppsExtensionsCompressed/rar.ico");

        public static readonly IImage KeyExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "CubeToolsAppsExtensionsCompressed/key.ico");

        public static readonly IImage MusicExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "CubeToolsAppsExtensionsCompressed/mp3.ico");

        public static readonly IImage VideoExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "CubeToolsAppsExtensionsCompressed/mp4.ico");

        public static readonly IImage DefaultExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "CubeToolsAppsExtensionsCompressed/None.ico");
    }
}