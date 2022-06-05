﻿using System;
using System.IO;

namespace LibraryClient.LibraryGoogleDrive
{
        
    public static class ManagerFile
    {
        public static string CreateFolder(string parent, string folderName)
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

        public static string CreateFile(string folderId, string fileName)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = new Google.Apis.Drive.v3.Data.File();
            
            DriveFile.Name = fileName;
            DriveFile.MimeType = "application/octet-stream";
            DriveFile.Parents = new[] {folderId};

            var command = Service.Files.Create(DriveFile);
            var file = command.Execute();
            return file.Id;
        }

        public static string UploadFile(Stream file, string fileName, string fileMime, string folder,
            string fileDescription)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = new Google.Apis.Drive.v3.Data.File();

            DriveFile.Name = fileName;
            DriveFile.MimeType = fileMime;
            DriveFile.Description = fileDescription;
            DriveFile.Parents = new string[] {folder};

            var request = Service.Files.Create(DriveFile, file, fileMime);
            request.Fields = "id";

            var response = request.Upload();
            if (response.Status != Google.Apis.Upload.UploadStatus.Completed) throw response.Exception;

            return request.ResponseBody.Id;
        }

        public static MemoryStream DownloadFile(string fileName)
        {
            var Service = OAuth.GetDriveService();
            var fileId = FileReader.GetFileId(fileName);

            var request = Service.Files.Get(fileId);
            var stream = new MemoryStream();

            request.MediaDownloader.ProgressChanged += (progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                    {
                        Console.WriteLine(progress.BytesDownloaded);
                        break;
                    }
                    case Google.Apis.Download.DownloadStatus.Completed:
                    {
                        Console.WriteLine("Download complete.");
                        break;
                    }
                    case Google.Apis.Download.DownloadStatus.Failed:
                    {
                        Console.WriteLine("Download failed.");
                        break;
                    }
                }
            };

            request.Download(stream);
            return stream;
        }

        public static void DeleteFile(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var command = Service.Files.Delete(fileId);
            command.Execute();
        }

        public static string CopyFile(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileId).Execute();

            string name = DriveFile.Name + "- Copy";

            while (FileReader.GetFileId(name, DriveFile.Parents[0]) != null)
            {
                name += "- Copy";
            }
            
            var copyFile = new Google.Apis.Drive.v3.Data.File();
            var command = Service.Files.Copy(copyFile, fileId).Execute();
            copyFile.Name = name;
            return copyFile.Id;
        }

        public static void ChangeParentsFile(string fileid, string newfolderid)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileid).Execute();
            
            DriveFile.Parents.Clear();
            DriveFile.Parents = new[] {newfolderid};

            Service.Files.Update(DriveFile, fileid).Execute();
        }

        public static void Rename(string fileid, string newName)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileid).Execute();

            DriveFile.Name = newName;

            Service.Files.Update(DriveFile, fileid).Execute();
        }
    }
}