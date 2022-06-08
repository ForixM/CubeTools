using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ResourcesLoader
{
    public static class ResourcesExtensions
    {

        public static readonly IImage ImageExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/jpg.ico");

        public static readonly IImage TextExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/pages.ico");

        public static readonly IImage ExeExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "Extensions/exe.ico");

        public static readonly IImage DocxExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/docx.ico");

        public static readonly IImage PdfExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "Extensions/pdf.ico");

        public static readonly IImage PptxExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/pptx.ico");

        public static readonly IImage PyExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "Extensions/py.ico");

        public static readonly IImage CsExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "Extensions/csharp.ico");

        public static readonly IImage CPlusPlusExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "Extensions/c++.ico");

        public static readonly IImage JavaExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/java.ico");

        public static readonly IImage HtmlExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/html.ico");

        public static readonly IImage XslsExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "Extensions/xlsx.ico");

        public static readonly IImage GitIgnoreExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "Extensions/gitignore.ico");

        public static readonly IImage CompressExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                     "Extensions/zip.ico");

        public static readonly IImage ArchiveExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "Extensions/rar.ico");

        public static readonly IImage KeyExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "Extensions/key.ico");

        public static readonly IImage MusicExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/mp3.ico");

        public static readonly IImage VideoExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/mp4.ico");
        public static readonly IImage DllExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/dll.ico");
        public static readonly IImage IniExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/ini.ico");
        public static readonly IImage BatExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/bat.ico");
        public static readonly IImage JsonExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/json.ico");
        public static readonly IImage RegisterExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "Extensions/reg.ico");

        public static readonly IImage DefaultExtension = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "Extensions/None.ico");
    }
}