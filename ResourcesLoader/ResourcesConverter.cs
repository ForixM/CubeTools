// using System.Drawing;

using System.Drawing;
using System.Drawing.Imaging;
using Avalonia;
using Avalonia.Media;
using Avalonia.Platform;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using PixelFormat = Avalonia.Platform.PixelFormat;

namespace ResourcesLoader
{
    public static class ResourcesConverter
    {
        public static IImage TypeToIcon(string path, string type, bool isdir = false)
        {
            if (isdir)
                return ResourcesIcons.FolderIcon;
            switch (type)
            {
                case "exe":
                    try
                    {
                        MemoryStream stream = new MemoryStream();
                        Icon.ExtractAssociatedIcon(path).ToBitmap().Save(stream, ImageFormat.Icon);
                        return new Bitmap(stream);
                    }
                    catch (Exception e)
                    {
                        return ResourcesExtensionsCompressed.ExeExtensionCompressed;
                    }
                // var bitmap = Icon.ExtractAssociatedIcon(path).ToBitmap();
                    // return new Bitmap(PixelFormat.Rgba8888, AlphaFormat.Opaque, bitmap.GetHicon(),
                    //     new PixelSize(bitmap.Width, bitmap.Height), new Vector(bitmap.Width, bitmap.Height), (int) bitmap.HorizontalResolution);
                case "jpg":
                case "jpeg":
                case "png" :
                case "ico":
                case "gif":
                    return ResourcesExtensionsCompressed.ImageExtensionCompressed;
                case "md":
                case "MD":
                case "txt":
                case "log":
                    return ResourcesExtensionsCompressed.TextExtensionCompressed;
                case "iso":
                    return ResourcesExtensionsCompressed.ExeExtensionCompressed;
                case "docx":
                case "doc":
                case "odt":
                    return ResourcesExtensionsCompressed.DocxExtensionCompressed;
                case "pdf":
                    return ResourcesExtensionsCompressed.PdfExtensionCompressed;
                case "py":
                    return ResourcesExtensionsCompressed.PyExtensionCompressed;
                case "cs":
                    return ResourcesExtensionsCompressed.CsExtensionCompressed;
                case "cpp":
                    return ResourcesExtensionsCompressed.CPlusPlusExtensionCompressed;
                case "java":
                case "class":
                    return ResourcesExtensionsCompressed.JavaExtensionCompressed;
                case "html":
                    return ResourcesExtensionsCompressed.HtmlExtensionCompressed;
                case "pptx":
                case "ppt":
                case "odp":
                    return ResourcesExtensionsCompressed.PptxExtensionCompressed;
                case "xlsx": 
                case "xls":
                case "csv":
                case "ods":
                    return ResourcesExtensionsCompressed.XlsxExtensionCompressed;
                case "gitignore":
                    return ResourcesExtensionsCompressed.GitIgnoreExtensionCompressed;
                case "7z":
                case "zip":
                    return ResourcesExtensionsCompressed.CompressExtensionCompressed;
                case "rar":
                    return ResourcesExtensionsCompressed.ArchiveExtensionCompressed;
                case "gdoc":
                    return ResourcesExtensionsCompressed.GoogleDocCompressed;
                case "gsheet":
                    return ResourcesExtensionsCompressed.GoogleSheetCompressed;
                case "gslide":
                    return ResourcesExtensionsCompressed.GoogleSlideCompressed;
                case "pages":
                    return ResourcesExtensionsCompressed.PagesCompressed;
                case "numbers":
                    return ResourcesExtensionsCompressed.NumbersCompressed;
                case "key":
                    return ResourcesExtensionsCompressed.KeyExtensionCompressed;
                case "mp3":
                case "m4a":
                case "alac":
                case "flac": 
                case "wav":
                    return ResourcesExtensionsCompressed.MusicExtensionCompressed;
                case "mp4":
                case "MP4":
                case "mov":
                case "MOV":
                case "mpeg":
                case "wmv":
                    return ResourcesExtensionsCompressed.VideoExtensionCompressed;
                case "xml":
                case "yaml": 
                case "yml":
                    return ResourcesExtensionsCompressed.XmlExtensionCompressed;
                case "reg":
                    return ResourcesExtensionsCompressed.RegisterExtensionCompressed;
                case "json":
                    return ResourcesExtensionsCompressed.JsonExtensionCompressed;
                case "ini":
                    return ResourcesExtensionsCompressed.IniExtensionCompressed;
                case "dll":
                    return ResourcesExtensionsCompressed.DllExtensionCompressed;
                case "bat":
                    return ResourcesExtensionsCompressed.BatExtensionCompressed;
                
                default:
                    return ResourcesExtensionsCompressed.DefaultExtensionCompressed;
            }
        }
    }
}