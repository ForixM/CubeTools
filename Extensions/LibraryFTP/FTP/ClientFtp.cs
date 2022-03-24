using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Library.Pointers;

namespace LibraryFTP;

public class ClientFtp
{
    private string _host = "ftp://";
    private string _username;
    private string _password;
    
    public delegate void FinishEventHandler(object sender, bool success);
    public delegate void TransfertEventHandler(object sender, long bytesTransfered, long totalBytes);

    public event FinishEventHandler finish;
    public event TransfertEventHandler progress;

    public ClientFtp(string host, string username, string password)
    {
        this._username = username;
        this._password = password;
        if (host[^1] is '/')
        {
            this._host += host.Remove(host.Length - 1, 1);
        }
        else
        {
            this._host += host;
        }
    }

    public FtpArboresence ListDirectory(FtpFolder folder)
    {
        FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host+folder.Path);
        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        request.Credentials = new NetworkCredential(_username, _password);

        FtpWebResponse response = (FtpWebResponse) request.GetResponse();
        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);
        FtpArboresence arbo = new FtpArboresence();
        do
        {
            try
            {
                Regex regex = new Regex ( @"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );
                MatchCollection matches = regex.Matches(reader.ReadLine());
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups[1].Value == "d")
                    {
                        FtpFolder f = new FtpFolder(groups[^1].Value, folder.Path+"/");
                        arbo.Items.Add(f);
                    }
                    else if (groups[1].Value == "-")
                    {
                        FtpFile file = new FtpFile(groups[^1].Value, Int32.Parse(groups[3].Value), folder.Path+"/");
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

    public FtpArboresence ListRoot()
    {
        FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host);
        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        request.Credentials = new NetworkCredential(_username, _password);

        FtpWebResponse response = (FtpWebResponse) request.GetResponse();

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);
        FtpArboresence arbo = new FtpArboresence();
        do
        {
            try
            {
                Regex regex = new Regex ( @"^([d-])([rwxt-]{3}){3}\s+\d{1,}\s+.*?(\d{1,})\s+(\w+\s+\d{1,2}\s+(?:\d{4})?)(\d{1,2}:\d{2})?\s+(.+?)\s?$",
                    RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace );
                MatchCollection matches = regex.Matches(reader.ReadLine());
                foreach (Match match in matches)
                {
                    GroupCollection groups = match.Groups;
                    if (groups[1].Value == "d")
                    {
                        FtpFolder folder = new FtpFolder(groups[^1].Value, "/");
                        arbo.Items.Add(folder);
                    }
                    else if (groups[1].Value == "-")
                    {
                        FtpFile file = new FtpFile(groups[^1].Value, Int32.Parse(groups[3].Value), "/");
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

        FtpWebResponse response = (FtpWebResponse) request.GetResponse();
        using (Stream ftpStream = response.GetResponseStream())
        using (Stream fileStream = File.Create(destination))
        {
            byte[] buffer = new byte[4096];
            int read;
            while ((read = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                fileStream.Write(buffer, 0, read);
                progress?.Invoke(this, fileStream.Position, file.Size);
            }
        }
        finish?.Invoke(this,(int)response.StatusCode == 200);
    }

    public void UploadFile(FileType file, FtpFolder destination)
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

    public void MakeDirectory(FtpFolder parent, string name)
    {
        FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + parent.Path + name);
        request.Method = WebRequestMethods.Ftp.MakeDirectory;
        request.Credentials = new NetworkCredential(_username, _password);
    }

    public void DeleteItem(IFtpItem item)
    {
        FtpWebRequest request = (FtpWebRequest) WebRequest.Create(_host + item.Path);
        if (item is FtpFolder)
        {
            request.Method = WebRequestMethods.Ftp.RemoveDirectory;
            FtpArboresence arbo = ListDirectory((FtpFolder) item);
            foreach (IFtpItem arboItem in arbo.Items)
            {
                DeleteItem(arboItem);
            }
        }
        if (item is FtpFile)
        {
            request.Method = WebRequestMethods.Ftp.DeleteFile;
        }
        request.Credentials = new NetworkCredential(_username, _password);
        FtpWebResponse response = (FtpWebResponse) request.GetResponse();
    }

    public void MoveFile(FtpFile file, FtpFolder destination) //TODO Implement
    {
        throw new NotImplementedException();
    }

    public void MoveDirectory(FtpFolder folder, FtpFolder destination) //TODO Implement
    {
        throw new NotImplementedException();
    }

    public void Rename(IFtpItem item, string newName) //TODO Implement
    {
        throw new NotImplementedException();
    }
}