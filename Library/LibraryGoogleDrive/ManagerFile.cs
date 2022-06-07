using System;
using System.IO;

namespace Library.LibraryGoogleDrive
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

        public static string UploadFile(string path, string fileName, string fileMime, string folder,
            string fileDescription = "")
        {
            Stream file = System.IO.File.OpenRead(@path);
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

        public static void DownloadFile(string fileId, string path)
        {
            var Service = OAuth.GetDriveService();
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
            var driveFile = request.Execute();
            FileStream file = new FileStream(path + "/" + driveFile.Name, FileMode.Create, FileAccess.Write);
            stream.WriteTo(file);
            file.Close();
            stream.Close();
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

        public static string ChangeParentsFile(string fileId, string folderId)
        {
            var Service = OAuth.GetDriveService();
            var request = Service.Files.Get(fileId);
            request.Fields = "parents";
            var file = request.Execute();
            var previousParents = String.Join(",", file.Parents);
            var updateRequest =
                Service.Files.Update(new Google.Apis.Drive.v3.Data.File(),
                    fileId);
            updateRequest.Fields = "id, parents";
            updateRequest.AddParents = folderId;
            updateRequest.RemoveParents = previousParents;
            file = updateRequest.Execute();

            return file.Parents[0];
        }

        public static void Rename(string fileid, string newName)
        {
            var Service = OAuth.GetDriveService();
            var file = new Google.Apis.Drive.v3.Data.File() {Name = newName};
            var update = Service.Files.Update(file, fileid);
            update.Fields = "name";
            file = update.Execute();
        }
    }
}
