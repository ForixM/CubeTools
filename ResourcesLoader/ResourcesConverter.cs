using Avalonia.Media;

namespace ResourcesLoader
{
    public static class ResourcesConverter
    {
        public static IImage TypeToIcon(string type, bool isdir = false)
        {
            if (isdir)
                return ResourcesIcons.FolderIcon;
            switch (type)
            {
                case "jpg":
                case "jpeg":
                case "png" :
                case "ico":
                    return ResourcesExtensionsCompressed.ImageExtensionCompressed;
                case "txt":
                    return ResourcesExtensionsCompressed.TextExtensionCompressed;
                case "exe":
                case "iso":
                    return ResourcesExtensionsCompressed.ExeExtensionCompressed;
                case "docx":
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
                    return ResourcesExtensionsCompressed.JavaExtensionCompressed;
                case "html":
                    return ResourcesExtensionsCompressed.HtmlExtensionCompressed;
                case "pptx":
                    return ResourcesExtensionsCompressed.PptxExtensionCompressed;
                case "xlsx":
                    return ResourcesExtensionsCompressed.XlsxExtensionCompressed;
                case "gitignore":
                    return ResourcesExtensionsCompressed.GitIgnoreExtensionCompressed;
                case "7z":
                case "zip":
                    return ResourcesExtensionsCompressed.CompressExtensionCompressed;
                case "rar":
                    return ResourcesExtensionsCompressed.ArchiveExtensionCompressed;
                case "key":
                    return ResourcesExtensionsCompressed.KeyExtensionCompressed;
                case "mp3":
                case "alac":
                case "flac": 
                case "wav":
                    return ResourcesExtensionsCompressed.MusicExtensionCompressed;
                case "mp4":
                case "mov":
                case "mpeg":
                    return ResourcesExtensionsCompressed.VideoExtensionCompressed;
                default:
                    return ResourcesExtensionsCompressed.DefaultExtensionCompressed;
            }
        }
    }
}