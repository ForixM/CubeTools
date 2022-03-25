using System;
using System.IO;

namespace LibraryGoogleDrive;

public class GoogleDriveFile
{
    public GoogleDriveFile()
    {
        fileName = "";
        fileMime = "";
        folder = "";
        fileDescription = "";
        fileID = "";
    }

    public Stream file { get; private set; }
    public string fileName { get; set; }
    public string fileMime { get; set; }
    public string folder { get; set; }
    public string fileDescription { get; set; }
    public string fileID { get; set; }

    public static string CreateFolder(string parent, string folderName)
    {
        var Service = OAuth.GetDriveService();
        var DriveFolder = new Google.Apis.Drive.v3.Data.File();

        DriveFolder.Name = folderName;
        DriveFolder.MimeType = "application/vnd.google-apps.folder";
        DriveFolder.Parents = new[] {parent};

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
        DriveFile.Parents = new[] {folder};

        var request = Service.Files.Create(DriveFile, file, fileMime);
        request.Fields = "id";

        var response = request.Upload();
        if (response.Status != Google.Apis.Upload.UploadStatus.Completed) throw response.Exception;

        return request.ResponseBody.Id;
    }

    public void DownloadFile(string fileName)
    {
        var Service = OAuth.GetDriveService();
        var fileId = FileReader.GetFileId(fileName);

        var request = Service.Files.Get(fileId);
        var stream = new MemoryStream();

        request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
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
                    //SaveStream(stream, saveTo);
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
    }

    public static void DeleteFile(string fileId)
    {
        var Service = OAuth.GetDriveService();
        var command = Service.Files.Delete(fileId);
        command.Execute();
    }

    public static string CopyFile(Stream file, string fileID)
    {
        var Service = OAuth.GetDriveService();
        var CopyFile = new Google.Apis.Drive.v3.Data.File();
        var command = Service.Files.Copy(CopyFile, fileID);
        command.Execute();
        return CopyFile.Id;
    }
}