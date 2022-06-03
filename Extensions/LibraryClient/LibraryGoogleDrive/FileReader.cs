using System;
using System.Collections.Generic;
using Google.Apis.Drive.v3;

namespace LibraryClient.LibraryGoogleDrive
{
    public static class FileReader
    {
        public static string GetFolderId(string FolderName, string Parent)
        {
            var service = OAuth.GetDriveService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{FolderName}' and {Parent}' in parents";
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

                if (files.Count == 0)
                {
                    return null;
                }

                return files[0].Id;
        }

        public static string GetFileId(string FileName, string Parent = "")
        {
            var service = OAuth.GetDriveService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and name = '{FileName}' and '{Parent}' in parents";
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

            if (files.Count == 0)
            {
                return null;
            }

            return files[0].Id;
        }

        public static List<string> GetFileProperties(string fileid)
        {
            var service = OAuth.GetDriveService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.DriveId = fileid;
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            if (files.Count == 0)
            {
                throw new Exception("File doesn't exist");
            }

            List<string> fileproperties = new List<string>();
            fileproperties.Add(files[0].Name);
            fileproperties.Add(files[0].MimeType);
            fileproperties.Add(files[0].Size.ToString());
            fileproperties.Add(files[0].Parents[0]);

            return fileproperties;
        }
        
        public static List<Google.Apis.Drive.v3.Data.File> ListFileAndFolder(string folderID)
        {
            var service = OAuth.GetDriveService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";
            listRequest.Q = $"'{folderID}' in parents";

            var list = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                listRequest.PageToken = pageToken;
                var result = listRequest.Execute();
                var files = result.Files;
                pageToken = result.NextPageToken;
                list.AddRange(files);
            } while (pageToken != null);

            return list;
        }
        
        public static List<Google.Apis.Drive.v3.Data.File> ListFile(string folderID)
        {
            var service = OAuth.GetDriveService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";
            listRequest.Q = $"mimeType!='application/vnd.google-apps.folder' and '{folderID}' in parents";

            var list = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                listRequest.PageToken = pageToken;
                var result = listRequest.Execute();
                var files = result.Files;
                pageToken = result.NextPageToken;
                list.AddRange(files);
            } while (pageToken != null);

            return list;
        }
        
        public static List<Google.Apis.Drive.v3.Data.File> ListFolder(string folderID)
        {
            var service = OAuth.GetDriveService();

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";
            listRequest.Q = $"mimeType=='application/vnd.google-apps.folder' and '{folderID}' in parents";

            var list = new List<Google.Apis.Drive.v3.Data.File>();
            string pageToken = null;
            do
            {
                listRequest.PageToken = pageToken;
                var result = listRequest.Execute();
                var files = result.Files;
                pageToken = result.NextPageToken;
                list.AddRange(files);
            } while (pageToken != null);

            return list;
        }

        public static string GetFileIdFromPath(string path)
        {
            string[] pathList = path.Split('/');

            string parent = "root";
            
            for (int i = 0; i < pathList.Length - 1; i++)
            {
                parent = GetFolderId(pathList[i], parent);
            }

            return GetFileId(pathList[^1], parent);
        }

        public static string GetFileName(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.Name;
        }
        
        public static string GetFileType(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.MimeType;
        }
        
        
        public static string GetFileDescription(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.Description;
        }
        
        public static string GetFileKind(string fileId)
        {
            var Service = OAuth.GetDriveService();
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.Kind;
        }
    }
}
