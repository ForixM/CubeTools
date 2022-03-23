using System;
using System.IO;
using System.Net;

namespace LibraryFTP;

public class ClientFtp
{
    private string _username;
    private string _pasword;
    
    public ClientFtp(string username, string password)
    {
        this._username = username;
        this._pasword = password;
    }

    public FtpArboresence ListDirectory()
    {
        FtpWebRequest request = (FtpWebRequest) WebRequest.Create("ftp://127.0.0.1/");
        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
        request.Credentials = new NetworkCredential(_username, _pasword);

        FtpWebResponse response = (FtpWebResponse) request.GetResponse();
        if ((int) response.StatusCode != 200) throw new Exception("Error");

        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);
        FtpArboresence arbo = new FtpArboresence();
        do
        {
            try
            {
                string[] args = reader.ReadLine().Split(' ');
                int length = args.Length;
                foreach (string s in args)
                {
                    if (s == "")
                    {
                        length--;
                    }
                }

                string[] newargs = new string[length];
                int i = 0;
                foreach (string s in args)
                {
                    if (s != "")
                    {
                        newargs[i] = s;
                        i++;
                    }
                }
                FtpFile file = new FtpFile(newargs[^1], Int32.Parse(newargs[4]));
                arbo.Files.Add(file);
            }
            catch (Exception e)
            {
                break;
            }

        } while (true);
        
        reader.Close();
        response.Close();
        return arbo;
    }
}