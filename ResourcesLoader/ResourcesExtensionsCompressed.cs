using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ResourcesLoader
{
    public static class ResourcesExtensionsCompressed
    {

        public static IImage ImageExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/jpg.ico");

        public static IImage TextExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/txt.ico");

        public static IImage ExeExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/exe.ico");

        public static IImage DocxExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/docx.ico");

        public static IImage PdfExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/pdf.ico");

        public static IImage PptxExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/pptx.ico");

        public static IImage PyExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "ExtensionsCompressed/py.ico");

        public static IImage CsExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "ExtensionsCompressed/csharp.ico");

        public static IImage CPlusPlusExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "ExtensionsCompressed/c++.ico");

        public static IImage JavaExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/java.ico");

        public static IImage HtmlExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/html.ico");

        public static IImage XlsxExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/xlsx.ico");

        public static IImage GitIgnoreExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "ExtensionsCompressed/gitignore.ico");

        public static IImage CompressExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                     "ExtensionsCompressed/zip.ico");

        public static IImage ArchiveExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "ExtensionsCompressed/rar.ico");

        public static IImage KeyExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/key.ico");

        public static IImage MusicExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/mp3.ico");

        public static IImage VideoExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/mp4.ico");
        
        public static IImage XmlExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/code.ico");
        public static IImage DllExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/dll.ico");
        public static IImage IniExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                          "ExtensionsCompressed/ini.ico");
        public static IImage BatExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                          "ExtensionsCompressed/bat.ico");
        public static IImage JsonExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                           "ExtensionsCompressed/json.ico");
        public static IImage RegisterExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                               "ExtensionsCompressed/reg.ico");
        public static IImage GoogleDocCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                       "ExtensionsCompressed/gdoc.ico");
        public static IImage GoogleSheetCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                         "ExtensionsCompressed/gsheet.ico");
        public static IImage GoogleSlideCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                         "ExtensionsCompressed/gslide.ico");
        public static IImage PagesCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                   "ExtensionsCompressed/pages.ico");
        public static IImage NumbersCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                     "ExtensionsCompressed/numbers.ico");

        public static IImage DefaultExtensionCompressed => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "ExtensionsCompressed/None.ico");
        
    }
}