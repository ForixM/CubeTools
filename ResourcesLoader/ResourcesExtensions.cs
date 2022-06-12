using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ResourcesLoader
{
    public static class ResourcesExtensions
    {

        public static IImage ImageExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/jpg.ico");

        public static IImage TextExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/pages.ico");

        public static IImage ExeExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "Extensions/exe.ico");

        public static IImage DocxExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/docx.ico");

        public static IImage PdfExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "Extensions/pdf.ico");

        public static IImage PptxExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/pptx.ico");

        public static IImage PyExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "Extensions/py.ico");

        public static IImage CsExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "Extensions/csharp.ico");

        public static IImage CPlusPlusExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "Extensions/c++.ico");

        public static IImage JavaExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/java.ico");

        public static IImage HtmlExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/html.ico");

        public static IImage XslsExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/xlsx.ico");

        public static IImage GitIgnoreExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "Extensions/gitignore.ico");

        public static IImage CompressExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                     "Extensions/zip.ico");

        public static IImage ArchiveExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "Extensions/rar.ico");

        public static IImage KeyExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "Extensions/key.ico");

        public static IImage MusicExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/mp3.ico");

        public static IImage VideoExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/mp4.ico");
        public static IImage DllExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/dll.ico");
        public static IImage IniExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/ini.ico");
        public static IImage BatExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/bat.ico");
        public static IImage JsonExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/json.ico");
        public static IImage RegisterExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/reg.ico");
        public static IImage GoogleDoc => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                             "Extensions/gdoc.ico");
        public static IImage GoogleSheet => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "Extensions/gsheet.ico");
        public static IImage GoogleSlide => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "Extensions/gslide.ico");
        public static IImage Pages => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                         "Extensions/pages.ico");
        public static IImage Numbers => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                           "Extensions/numbers.ico");

        public static IImage DefaultExtension => new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "Extensions/None.ico");
        
    }
}