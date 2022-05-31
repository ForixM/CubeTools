using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace ResourcesLoader
{
    public static class ResourcesExtensionsCompressed
    {

        public static readonly IImage ImageExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/jpg.ico");

        public static readonly IImage TextExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/pages.ico");

        public static readonly IImage ExeExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/exe.ico");

        public static readonly IImage DocxExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/docx.ico");

        public static readonly IImage PdfExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/pdf.ico");

        public static readonly IImage PptxExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/pptx.ico");

        public static readonly IImage PyExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "ExtensionsCompressed/py.ico");

        public static readonly IImage CsExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                               "ExtensionsCompressed/csharp.ico");

        public static readonly IImage CPlusPlusExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "ExtensionsCompressed/c++.ico");

        public static readonly IImage JavaExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/java.ico");

        public static readonly IImage HtmlExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/html.ico");

        public static readonly IImage XlsxExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                 "ExtensionsCompressed/xlsx.ico");

        public static readonly IImage GitIgnoreExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                      "ExtensionsCompressed/gitignore.ico");

        public static readonly IImage CompressExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                     "ExtensionsCompressed/zip.ico");

        public static readonly IImage ArchiveExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "ExtensionsCompressed/rar.ico");

        public static readonly IImage KeyExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                "ExtensionsCompressed/key.ico");

        public static readonly IImage MusicExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/mp3.ico");

        public static readonly IImage VideoExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                  "ExtensionsCompressed/mp4.ico");

        public static readonly IImage DefaultExtensionCompressed = new Bitmap(ConfigLoader.ConfigLoader.Settings.ResourcePath +
                                                                    "ExtensionsCompressed/None.ico");
    }
}