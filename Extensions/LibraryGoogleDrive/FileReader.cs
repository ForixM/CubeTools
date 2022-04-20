using System;
using System.Collections.Generic;
using Google.Apis.Drive.v3;

namespace LibraryGoogleDrive
{
    
public static class FileReader
{
    public static string GetFolderId(string FolderName)
    {
        var service = OAuth.GetDriveService();

        FilesResource.ListRequest listRequest = service.Files.List();
        listRequest.PageSize = 10;
        listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{FolderName}'";
        listRequest.Fields = "nextPageToken, files(id, name)";

        IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
            .Files;

        return files[0].Id;
    }

    public static string GetFileId(string FileName)
    {
        var service = OAuth.GetDriveService();

        FilesResource.ListRequest listRequest = service.Files.List();
        listRequest.PageSize = 10;
        listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and name = '{FileName}'";
        listRequest.Fields = "nextPageToken, files(id, name)";

        IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
            .Files;

        if (files.Count == 0)
        {
            throw new Exception();
        }

        return files[0].Id;
    }

    public static void ListFileAndFolder(string folderID)
    {
        var service = OAuth.GetDriveService();

        FilesResource.ListRequest listRequest = service.Files.List();
        listRequest.PageSize = 100;
        listRequest.Fields = "nextPageToken, files(id, name)";
        listRequest.Q = $"'{folderID}' in parents";

        // List files.
        IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
            .Files;
        Console.WriteLine("Files:");
        if (files != null && files.Count > 0)
            foreach (var file in files)
                Console.WriteLine("{0} ({1})", file.Name, file.Id);
        else
            Console.WriteLine("No files found.");
        Console.Read();
    }
}
}
