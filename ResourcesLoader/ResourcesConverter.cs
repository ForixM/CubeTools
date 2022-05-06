using Avalonia.Media;
using Library.Pointers;

namespace ResourcesLoader
{
    public static class ResourcesConverter
    {
        public static IImage TypeToIcon(string type, bool isdir)
        {
            if (isdir)
                return ResourcesIcons.FolderIcon;
            switch (type)
            {
                case "jpg":
                case "jpeg":
                case "png" :
                case "ico":
                    return ResourcesExtensions.ImageExtension;
                case "txt":
                    return ResourcesExtensions.TextExtension;
                case "exe":
                case "iso":
                    return ResourcesExtensions.ExeExtension;
                case "docx":
                case "odt":
                    return ResourcesExtensions.DocxExtension;
                case "pdf":
                    return ResourcesExtensions.PdfExtension;
                case "py":
                    return ResourcesExtensions.PyExtension;
                case "cs":
                    return ResourcesExtensions.CsExtension;
                case "c++":
                    return ResourcesExtensions.CPlusPlusExtension;
                case "java":
                    return ResourcesExtensions.JavaExtension;
                case "html":
                    return ResourcesExtensions.HtmlExtension;
                case "pptx":
                    return ResourcesExtensions.PptxExtension;
                case "xlsx":
                    return ResourcesExtensions.XslsExtension;
                case "gitignore":
                    return ResourcesExtensions.GitIgnoreExtension;
                case "7z":
                case "zip":
                    return ResourcesExtensions.CompressExtension;
                case "rar":
                    return ResourcesExtensions.ArchiveExtension;
                case "key":
                    return ResourcesExtensions.KeyExtension;
                case "mp3":
                case "alac":
                case "flac": 
                case "wav":
                    return ResourcesExtensions.MusicExtension;
                case "mp4":
                case "mov":
                case "mpeg":
                    return ResourcesExtensions.VideoExtension;
                default:
                    return ResourcesExtensions.DefaultExtension;
            }
        }
    }
}