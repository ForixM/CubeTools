using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Library;
using Library.DirectoryPointer.DirectoryPointerLoaded;

namespace LibraryClient.LibraryFtp
{
        
    public class ClientFtp
    {
        private readonly string _host = "ftp://";
        private readonly string _username;
        private readonly string _password;

        public string Host => _host;
        public string Username => _username;
        public string Password => _password;
        
        public delegate void FinishEventHandler(object sender, bool success);
        public delegate void TransfertEventHandler(object sender, long bytesTransfered, long totalBytes);

        public event FinishEventHandler finish;
        public event TransfertEventHandler progress;

        public ClientFtp(string host, string username, string password)
        {
            _username = username;
            _password = password;
            if (host[^1] is '/') _host += host.Remove(host.Length - 1, 1);
            else _host += host;
        }

        public FtpArboresence ListDirectory(FtpFolder folder)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host+folder.Path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(_username, _password);

            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream!);
            FtpArboresence arbo = new FtpArboresence();
            do
            {
                try
                {
                    Regex regex = new Regex ( @"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );
                    MatchCollection matches = regex.Matches(reader.ReadLine() ?? string.Empty);
                    foreach (Match match in matches)
                    {
                        GroupCollection groups = match.Groups;
                        if (groups[1].Value == "d")
                        {
                            FtpFolder f = new FtpFolder(groups[^1].Value, folder, groups[^3].Value+groups[^2].Value);
                            arbo.Items.Add(f);
                        }
                        else if (groups[1].Value == "-")
                        {
                            FtpFile file = new FtpFile(groups[^1].Value, Int32.Parse(groups[3].Value), folder, groups[^3].Value+groups[^2].Value);
                            arbo.Items.Add(file);
                        }
                    }
                }
                catch (Exception e)
                {
                    reader.Close();
                    response.Close();
                    return arbo;
                }
            } while (true);
        }
        
        public FtpArboresence ListDirectory(string path)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host+path);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential(_username, _password);

            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream!);
            FtpArboresence arbo = new FtpArboresence();
            do
            {
                try
                {
                    Regex regex = new Regex ( @"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                        RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );
                    MatchCollection matches = regex.Matches(reader.ReadLine() ?? string.Empty);
                    foreach (Match match in matches)
                    {
                        GroupCollection groups = match.Groups;
                        if (groups[1].Value == "d")
                        {
                            FtpFolder f = new FtpFolder(groups[^1].Value, path, groups[^3].Value+groups[^2].Value);
                            arbo.Items.Add(f);
                        }
                        else if (groups[1].Value == "-")
                        {
                            FtpFile file = new FtpFile(groups[^1].Value, Int32.Parse(groups[3].Value), path, groups[^3].Value+groups[^2].Value);
                            arbo.Items.Add(file);
                        }
                    }
                }
                catch (Exception e)
                {
                    reader.Close();
                    response.Close();
                    return arbo;
                }
            } while (true);
        }

        public void DownloadFile(FtpFile file, string destination)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + file.Path);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential(_username, _password);

            destination += "/" + file.Name;
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            using (Stream ftpStream = response.GetResponseStream())
            using (Stream fileStream = File.Create(destination))
            {
                byte[] buffer = new byte[4096];
                int read;
                while ((read = ftpStream!.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fileStream.Write(buffer, 0, read);
                    progress?.Invoke(this, fileStream.Position, file.Size);
                }
            }
            finish?.Invoke(this,(int)response.StatusCode == 200);
        }

        public void UploadFolder(Pointer ft, FtpFolder destination)
        {
            if (ft.IsDir)
            {
                // Store directory into a Local variable for treatment
                DirectoryPointerLoaded tmp = new DirectoryPointerLoaded(ft.Path);
                // Upload each files to its location
                foreach (var item in tmp.ChildrenFiles.Where(item => !item.IsDir))
                    UploadFile(item, destination);
                // Store a new FTP Folder for treatment
                FtpFolder currentFolder = new FtpFolder(ft.Path);
                // Recursive call on sub-dirs for treatment
                foreach (var item in tmp.ChildrenFiles.Where(item => item.IsDir))
                {
                    CreateFolder(currentFolder, item.Name);
                    UploadFolder(item, currentFolder);
                }
            }
            else 
                UploadFile(ft, destination);
        }

        public void UploadFile(Pointer file, FtpFolder destination)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + destination.Path + Path.GetFileName(file.Path));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_username, _password);
            
            using (Stream fileStream = File.OpenRead(file.Path))
            using (Stream ftpStream = request.GetRequestStream())
            {
                byte[] buffer = new byte[4096];
                int read;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ftpStream.Write(buffer, 0, read);
                    progress?.Invoke(this, fileStream.Position, new FileInfo(file.Path).Length);
                }
            }
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            finish?.Invoke(this,(int) response.StatusCode == 200);
        }

        public FtpFolder CreateFolder(FtpFolder parent, string name)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + parent.Path + "/" + name);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(_username, _password);
            request.GetResponse();
            return (FtpFolder) GetItem(parent, name);
        }
        
        public void Delete(IFtpItem item)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + item.Path);
            if (item is FtpFolder folder)
            {
                request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpArboresence arbo = ListDirectory(folder);
                foreach (IFtpItem arboItem in arbo.Items)
                {
                    Delete(arboItem);
                }
            }
            if (item is FtpFile)
            {
                request.Method = WebRequestMethods.Ftp.DeleteFile;
            }
            request.Credentials = new NetworkCredential(_username, _password);
            request.GetResponse();
        }
        
        public void MoveItem(IFtpItem file, FtpFolder destination)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + file.Path);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.Credentials = new NetworkCredential(_username, _password);
            request.RenameTo = Uri.UnescapeDataString(destination.Path + "/" + file.Name);
            request.GetResponse();
        }
        
        public void Rename(IFtpItem item, string newName) //TODO Implement
        {
            string path = item.Path;
            path = path.Remove(path.Length - 1);
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + path);
            request.Method = WebRequestMethods.Ftp.Rename;
            request.Credentials = new NetworkCredential(_username, _password);
            request.RenameTo = newName;
            request.GetResponse();
        }
        
        public FtpFile CreateFile(FtpFolder destination, string fileName)
        {
            FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + destination.Path + fileName);
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential(_username, _password);
            
            //using (Stream fileStream = File.OpenRead(file.Path))
            using (Stream ftpStream = request.GetRequestStream())
            {
                byte[] buffer = new byte[4];
                ftpStream.Write(buffer, 0, 0);
                /*int read;
                while ((read = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ftpStream.Write(buffer, 0, read);
                    progress?.Invoke(this, fileStream.Position, new FileInfo(file.Path).Length);
                }*/
            }
            FtpWebResponse response = (FtpWebResponse) request.GetResponse();
            finish?.Invoke(this,(int) response.StatusCode == 200);
            return (FtpFile) GetItem(destination, fileName);
        }

        [CanBeNull]
        public IFtpItem GetItem(FtpFolder folder, string fileName) => ListDirectory(folder.Path).Items.FirstOrDefault(item => item.Name == fileName);
        public bool ItemExist(FtpFolder folder, IFtpItem item) => ListDirectory(folder).Items.Contains(item);
    }
}
