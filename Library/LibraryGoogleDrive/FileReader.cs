using System;
using System.Collections.Generic;
using Google.Apis.Drive.v3;

namespace Library.LibraryGoogleDrive
{
    public static class FileReader
    {
        public static string GetFolderId(string FolderName, string Parent = "")
        {
            var service = ClientGoogleDrive.Service;

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            if (Parent == "")
            {
                listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{FolderName}'";
            }
            else
            {
                listRequest.Q = $"mimeType = 'application/vnd.google-apps.folder' and name = '{FolderName}' and '{Parent}' in parents";
            }
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
            var service = ClientGoogleDrive.Service;

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.PageSize = 10;
            if (Parent == "")
            {
                listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and name = '{FileName}'";
            }
            else
            {
                listRequest.Q = $"mimeType != 'application/vnd.google-apps.folder' and name = '{FileName}' and '{Parent}' in parents";
            }
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute()
                .Files;

            if (files.Count == 0)
            {
                return null;
            }

            return files[0].Id;
        }

        public static List<Google.Apis.Drive.v3.Data.File> ListFileAndFolder(string folderID)
        {
            var service = ClientGoogleDrive.Service;

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
            var service = ClientGoogleDrive.Service;

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
            var service = ClientGoogleDrive.Service;

            FilesResource.ListRequest listRequest = service.Files.List();
            listRequest.Fields = "nextPageToken, files(id, name, size, mimeType)";
            listRequest.Q = $"mimeType='application/vnd.google-apps.folder' and '{folderID}' in parents";

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
            if (path == "")
            {
                return "root";
            }
            string[] pathList = path.Split('/');

            string parent = "root";

            int i = 0;

            if (pathList[0] == "")
            {
                i += 1;
            }
            
            for (; i < pathList.Length - 1; i++)
            {
                parent = GetFolderId(pathList[i], parent);
            }

            string fileId = GetFileId(pathList[^1], parent);

            if (fileId is null)
            {
                fileId = GetFolderId(pathList[^1], parent);
            }

            return fileId;
        }

        public static string GetFileName(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.Name;
        }
        
        public static string GetFileType(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.MimeType;
        }
        
        
        public static string GetFileDescription(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.Description;
        }
        
        public static string GetFileKind(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var DriveFile = Service.Files.Get(fileId).Execute();

            return DriveFile.Kind;
        }

        public static long GetFileSize(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var request = Service.Files.Get(fileId);
            request.Fields = "size";
            var driveFile = request.Execute();

            if (driveFile.Size != null) return driveFile.Size.Value;

            return 0;
        }

        public static string GetFileParent(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var request = Service.Files.Get(fileId);
            request.Fields = "parents";
            var DriveFile = request.Execute();

            return DriveFile.Parents[0];
        }
        
        public static string GetPathFromFile(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            string pathRes = "";
            
            var request = Service.Files.Get(fileId);
            request.Fields = "parents, name";
            var driveFile = request.Execute();

            while (true)
            {
                if (driveFile.Parents == null)
                {
                    return pathRes;
                }
                pathRes = '/' + driveFile.Name + pathRes;
                fileId = driveFile.Parents[0];
                request = Service.Files.Get(fileId);
                request.Fields = "parents, name";
                driveFile = request.Execute();
            }
        }

        public static List<string> GetParentsFromFile(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            List<string> parents = new List<string>();

            var request = Service.Files.Get(fileId);
            request.Fields = "parents, id";
            var driveFile = request.Execute();

            while (true)
            {
                if (driveFile.Parents == null)
                {
                    return parents;
                }
                fileId = driveFile.Parents[0];
                parents.Add(fileId);
                request = Service.Files.Get(fileId);
                request.Fields = "parents, id";
                driveFile = request.Execute();
            }
        }

        public static bool IsDir(string fileId)
        {
            var Service = ClientGoogleDrive.Service;
            var DriveFile = Service.Files.Get(fileId).Execute();

            if (DriveFile.MimeType == "application/vnd.google-apps.folder")
            {
                return true;
            }

            return false;
        }
    }
}
