using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace GoogleDriveApi
{
    internal class GoogleDriveFile
    {
        public Stream file { get; private set; }
        public string fileName { get; set; }
        public string fileMime { get; set; }
        public string folder { get; set; }
        public string fileDescription { get; set; }
        public string fileID { get; set; }

        public GoogleDriveFile()
        {
            fileName = "";
            fileMime = "";
            folder = "";
            fileDescription = "";
            fileID = "";
        }

        public static string CreateFolder (string parent, string folderName)
        {
            var Service = OAuth.GetDriveService();
            var DriveFolder = new Google.Apis.Drive.v3.Data.File();

            DriveFolder.Name = folderName;
            DriveFolder.MimeType = "application/vnd.google-apps.folder";
            DriveFolder.Parents = new string[] {parent};

            var command = Service.Files.Create(DriveFolder);
            var file = command.Execute();
            return file.Id;
        }

        public static string UploadFile (Stream file, string fileName, string fileMime, string folder, string fileDescription)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = new Google.Apis.Drive.v3.Data.File();

            DriveFile.Name = fileName;
            DriveFile.MimeType = fileMime;
            DriveFile.Description = fileDescription;
            DriveFile.Parents = new string[] { folder };

            var request = Service.Files.Create(DriveFile, file, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw response.Exception;
            }

            return request.ResponseBody.Id;
        }

        public void DeleteFile(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var command = Service.Files.Delete(fileId);
            command.Execute();
        }

        public string CopyFile(Stream file, string fileID)
        {
            var Service = OAuth.GetDriveService();
            var CopyFile = new Google.Apis.Drive.v3.Data.File();
            var command = Service.Files.Copy(CopyFile, fileID);
            command.Execute();
            return CopyFile.Id;
        }
    }
}
