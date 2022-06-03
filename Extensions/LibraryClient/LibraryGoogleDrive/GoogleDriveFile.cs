using System.Collections.Generic;
using System.IO;

namespace LibraryClient.LibraryGoogleDrive
{
    public class GoogleDriveFile
    {
        public string FileId { get; set; }
        
        public string FileName { get; set; }
        
        public string FileType { get; set; }
        
        public string Size { get; set; }
        
        public string Parent { get; set; }

        public GoogleDriveFile(string fileId)
        {
            List<string> file = FileReader.GetFileProperties(fileId);

            FileId = fileId;
            FileName = file[1];
            FileType = file[2];
            Size = file[3];
            Parent = file[4];

        }
    }
}