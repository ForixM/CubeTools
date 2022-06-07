using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using HeyRed.Mime;
using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.LibraryFtp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Library.LibraryOneDrive
{
    public enum SharePermission
    {
        READONLY,
        READWRITE
    }
    public class OnedriveClient
    {
        private HttpListener _listener;
        
        private static string _clientId = "044195b6-72dd-47b6-b904-c2e84618919a";
        private static string _redirectUri = "http://localhost:4040";

        private Token token;
        private string RootPath = "/drive/root:";
        private HttpClient _client;

        private static string _authorityUri =
            string.Format(
                "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=onedrive.readwrite%20offline_access&response_type=code&redirect_uri={1}",
                Uri.EscapeDataString(_clientId),
                Uri.EscapeDataString(_redirectUri));

        private static string _tokenUri = "https://login.live.com/oauth20_token.srf";
        private static string _api = "https://api.onedrive.com/v1.0";

        public delegate void SampleEventHandler(object sender, bool success);

        public delegate void TransfertUpdateHandler(object sender, int percent);

        public event SampleEventHandler authenticated;
        public event SampleEventHandler uploadFinished;
        public event SampleEventHandler downloadFinished;
        public event TransfertUpdateHandler uploadUpdate;
        public event TransfertUpdateHandler downloadUpdate;

        public OnedriveClient()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(_redirectUri+"/");
            _listener.Start();
            Thread thread = new Thread(Authenticator);
            thread.Start();
            authenticated += (sender, success) =>
            {
                _listener.Stop();
                _client = new HttpClient();
            };
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(_authorityUri) { UseShellExecute = true });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", _authorityUri);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", _authorityUri);
            }
        }

        public OneArboresence GetArboresence()
        {
            HttpClient client = new HttpClient();
            Task<string> responseString =
                client.GetStringAsync(_api + RootPath + ":/children?access_token=" + token.access_token +
                                      "&select=name,size,folder,file,parentReference,id");
            responseString.Wait();
            try
            {
                var temp = JsonConvert.DeserializeObject<OneArboresence>(responseString.Result);
                foreach (var item in temp!.items) item.SetVariables();
                return temp;
            }
            catch (JsonException e)
            {
                LogErrors.LogErrors.LogWrite("Unable to read arboresence for OneDrive",e);
                return null;
            }
        }
        
        public OneArboresence GetArboresence(OnePointer folder)
        {
            if (folder.Type != OneItemType.FOLDER) return null;
            HttpClient client = new HttpClient();
            Task<string> responseString =
                client.GetStringAsync(_api + folder.path + ":/children?access_token=" + token.access_token +
                                      "&select=name,size,folder,file,parentReference,id");
            responseString.Wait();
            try
            {
                var temp = JsonConvert.DeserializeObject<OneArboresence>(responseString.Result);
                foreach (var item in temp!.items) item.SetVariables();
                return temp;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool CreateFolder(string name)
        {
            HttpClient client = new HttpClient();
            string data = "{'name':'"+name+"','folder':{ },'@microsoft.graph.conflictBehavior':'rename'}";
            Task<HttpResponseMessage> response =
                client.PostAsync(_api + RootPath + ":/children?access_token=" + token.access_token,
                    new StringContent(data, Encoding.UTF8, "application/json"));
            response.Wait();
            Task<string> resStr = response.Result.Content.ReadAsStringAsync();
            resStr.Wait();
            return (int) response.Result.StatusCode == 201;
        }
        
        public OnePointer CreateFolder(string name, OnePointer folder)
        {
            if (folder.Type != OneItemType.FOLDER)
                return null;
            string data = "{'name':'"+name+"','folder':{ },'@microsoft.graph.conflictBehavior':'rename'}";
            Task<HttpResponseMessage> response =
                _client.PostAsync(
                    _api + folder.path + ":/children?access_token=" +
                    token.access_token,
                    new StringContent(data, Encoding.UTF8, "application/json"));
            response.Wait();
            if ((int)response.Result.StatusCode == 201)
            {
                OneArboresence arbo = GetArboresence(folder);
                foreach (OnePointer item in arbo.items)
                {
                    if (item.name == name && item.IsDir)
                    {
                        return item;
                    }
                }
            }

            return null;
        }


        public async Task<bool> UploadFile(FileLocalPointer fileLocal)
        {
            HttpResponseMessage response = await _client.PutAsync(
                _api + RootPath + "/" + Path.GetFileName(fileLocal.Path) + ":/content?access_token=" + token.access_token,
                new StringContent(System.IO.File.ReadAllText(fileLocal.Path), Encoding.UTF8,
                    MimeTypesMap.GetMimeType(fileLocal.Path)));
            uploadFinished?.Invoke(this, (int)response.StatusCode == 201);
            return (int) response.StatusCode == 201;
        }

        public async Task<bool> UploadFile(LocalPointer file, OnePointer destination)
        {
            if (destination.Type != OneItemType.FOLDER) return false;
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(_api + destination.path +
                                                                            Path.GetFileName(file.Path) +
                                                                            ":/content?access_token=" +
                                                                            token.access_token);
            request.Method = "PUT";
            request.ContentType = MimeTypesMap.GetMimeType(file.Path);
            request.AllowWriteStreamBuffering = false;
            Stream fileStream = new FileStream(file.Path, FileMode.Open);
            request.ContentLength = fileStream.Length;
            Stream serverStream = request.GetRequestStream();

            byte[] buffer = new byte[4096];
            int uploaded = 0;
            int bytesRead;
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                serverStream.Write(buffer, 0, bytesRead);
                uploaded += bytesRead;
                uploadUpdate?.Invoke(this, (int) ((100*uploaded) / request.ContentLength));
            }

            serverStream.Close();
            fileStream.Close();
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            Console.WriteLine("Finished");
            uploadFinished?.Invoke(this,  (int)response.StatusCode == 201);
            return (int) response.StatusCode == 201;
        }
        
        public async Task<bool> UploadFolder(LocalPointer folder, OnePointer destination)
        {
            if (!destination.IsDir) return false;
            OnePointer newFolder = CreateFolder(folder.Name,
                destination);
            foreach (string file in Directory.GetFiles(folder.Path))
            {
                UploadFile(new FileLocalPointer(file), newFolder);
            }
            foreach (string directory in Directory.GetDirectories(folder.Path))
            {
                UploadFolder(new DirectoryLocalPointer(directory+@"\"), newFolder);
            }
            return true;
        }
        
        public async Task<bool> CreateFile(string fileName, OnePointer destination)
        {
            if (destination.Type != OneItemType.FOLDER) return false;
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(_api + destination.path +
                                                                            fileName +
                                                                            ":/content?access_token=" +
                                                                            token.access_token);
            request.Method = "PUT";
            request.ContentType = MimeTypesMap.GetMimeType(fileName);
            request.AllowWriteStreamBuffering = false;
            
            Stream serverStream = request.GetRequestStream();
            serverStream.Write(Array.Empty<byte>(), 0, 0);
            serverStream.Close();
            
            HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            uploadFinished?.Invoke(this,  (int)response.StatusCode == 201);
            return (int) response.StatusCode == 201;
        }

        public FileLocalPointer DownloadFile(string dest, OnePointer pointer)
        {
            HttpWebRequest request =
                (HttpWebRequest) HttpWebRequest.Create(
                    _api + pointer.path + ":/content?access_token=" + token.access_token);
            request.Method = "GET";
            request.AllowWriteStreamBuffering = false;
            Stream filestream = new FileStream(dest, FileMode.Create);
            WebResponse response = request.GetResponse();
            Stream serverStream = response.GetResponseStream();
            byte[] buffer = new byte[4096];
            long downloaded = 0;
            int bytesRead;
            while ((bytesRead = serverStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                filestream.Write(buffer, 0, bytesRead);
                downloaded += bytesRead;
                downloadUpdate?.Invoke(this, (int) ((100*downloaded) / response.ContentLength));
            }
            
            serverStream.Close();
            filestream.Close();
            HttpWebResponse webResponse = (HttpWebResponse) response;
            downloadFinished?.Invoke(this, (int)webResponse.StatusCode == 302); //TODO handle success bool value
            
            // Task<HttpResponseMessage> response =
            //     _client.GetAsync(_api + item.parentReference.path + "/" + item.name + ":/content?access_token=" +
            //                      token.access_token);
            // response.Wait();
            // Task<byte[]> resStr = response.Result.Content.ReadAsByteArrayAsync();
            // resStr.Wait();
            // File.WriteAllBytes(dest, resStr.Result);
            // downloadFinished?.Invoke(this, true); //TODO handle success bool value
            return new FileLocalPointer(dest);
        }
        
        public FileLocalPointer DownloadFile(string dest, string path)
        {
            if (path.StartsWith("./"))
            {
                path = path.Remove(0, 2);
                path = this.RootPath + path;
            }
            else if (path.StartsWith("/"))
            {
                
            }
            else
            {
                Console.WriteLine("Invalid path syntax");
                return null;
            }
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + path + ":/content?access_token=" + token.access_token);
            response.Wait();
            Task<byte[]> resStr = response.Result.Content.ReadAsByteArrayAsync();
            resStr.Wait();
            File.WriteAllBytes(dest, resStr.Result);
            uploadFinished?.Invoke(this, true); //TODO handle success bool value
            return new FileLocalPointer(dest);
        }
        
        public string DownloadFolder(string dest, OnePointer folder)
        {
            if (!folder.IsDir) return null;
            Directory.CreateDirectory(dest + folder.name);
            OneArboresence arbo = GetArboresence(folder);
            foreach (OnePointer item in arbo.items)
            {
                if (!item.IsDir)
                    DownloadFile(dest+folder.name+"/"+item.name, item);
                else
                {
                    DownloadFolder(dest+folder.name+"/", item);
                }
            }
            return dest;
        }

        public bool DeleteItem(OnePointer pointer)
        {
            Task<HttpResponseMessage> response =
                _client.DeleteAsync(_api + pointer.path + "?access_token=" +
                                    token.access_token);
            response.Wait();
            return (int) response.Result.StatusCode == 204;
        }
        
        public bool DeleteItem(string path)
        {
            Task<HttpResponseMessage> response =
                _client.DeleteAsync(_api + this.RootPath + path + "?access_token=" + token.access_token);
            response.Wait();
            return (int) response.Result.StatusCode == 204;
        }
        
        public bool RenameItem(OnePointer pointer, string newname)
        {
            JObject body = new JObject();
            body.Add(new JProperty("parentReference", new JObject(new JProperty("id", pointer.parentReference.id))));
            body.Add(new JProperty("name", newname));
            Task<HttpResponseMessage> response =
                _client.PatchAsync(
                    _api + pointer.path + "?access_token=" + token.access_token,
                    new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            response.Wait();
            if ((int)response.Result.StatusCode == 200)
            {
                pointer.name = newname;
                return true;
            }

            return false;
        }

        public bool MoveItem(OnePointer pointer, OnePointer destination)
        {
            if (destination.Type != OneItemType.FOLDER) return false;
            JObject body = new JObject();
            body.Add(new JProperty("parentReference", new JObject(new JProperty("id", destination.id))));
            body.Add(new JProperty("name", pointer.name));
            Task<HttpResponseMessage> response =
                _client.PatchAsync(
                    _api + pointer.path + "?access_token=" + token.access_token,
                    new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            response.Wait();
            return (int) response.Result.StatusCode == 200;
        }

        public JObject GetItemFullMetadata(OnePointer pointer)
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + pointer.path + "?access_token=" + token.access_token);
            response.Wait();
            Task<string> strResponse = response.Result.Content.ReadAsStringAsync();
            strResponse.Wait();
            return (int)response.Result.StatusCode==200 ? JObject.Parse(strResponse.Result) : null;
        }
        
        public JObject GetItemFullMetadata(string path)
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + path + "?access_token=" + token.access_token);
            response.Wait();
            Task<string> strResponse = response.Result.Content.ReadAsStringAsync();
            strResponse.Wait();
            return (int)response.Result.StatusCode==200 ? JObject.Parse(strResponse.Result) : null;
        }

        public JObject CreateShareLink(OnePointer pointer, SharePermission permission) //TODO Not working
        {
            JObject body = new JObject();
            body.Add(new JProperty("type", permission == SharePermission.READONLY ? "view" : "edit"));
            body.Add(new JProperty("scope", "anonymous"));
            Task<HttpResponseMessage> response =
            _client.PostAsync(_api + pointer.path + ":/oneDrive.createLink?access_token=" + token.access_token,
            new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            response.Wait();
            Task<string> strResponse = response.Result.Content.ReadAsStringAsync();
            strResponse.Wait();
            JObject data = GetItemFullMetadata(pointer);
            string link = data.GetValue("webUrl").ToString();
            Console.WriteLine(strResponse.Result);
            return (int)response.Result.StatusCode is 200 or 201 ? JObject.Parse(strResponse.Result) : null;
        }

        public JObject GetSharedItems()
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + "/drive/sharedWithMe?access_token=" + token.access_token);
            response.Wait();
            Task<string> str = response.Result.Content.ReadAsStringAsync();
            str.Wait();
            return JObject.Parse(str.Result);
        }

        public JObject GetPermissions(OnePointer pointer)
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + pointer.path + ":/permissions?access_token=" + token.access_token);
            response.Wait();
            Task<string> str = response.Result.Content.ReadAsStringAsync();
            str.Wait();
            return JObject.Parse(str.Result);
        }

        public bool Copy(OnePointer pointer, OnePointer newPath)
        {
            if (newPath.Type != OneItemType.FOLDER) return false;
            JObject body = new JObject();
            JObject parentReference = new JObject();
            parentReference.Add("driveId", pointer.parentReference.driveId);
            parentReference.Add("id", newPath.id);
            body.Add("parentReference", parentReference);
            body.Add("name", "test.docx");
            Console.WriteLine(body);
            Task<HttpResponseMessage> response =
                _client.PostAsync(_api + pointer.path + ":/oneDrive.copy?access_token="+token.access_token,
                    new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            response.Wait();
            Task<string> str = response.Result.Content.ReadAsStringAsync();
            str.Wait();
            Console.WriteLine(str.Result);
            return (int)response.Result.StatusCode == 202;
        }

        private void Authenticator()
        {
            HttpListenerContext context = _listener.GetContext();
            Uri uri = context.Request.Url;
            NameValueCollection param = HttpUtility.ParseQueryString(uri.Query);
            byte[] _responseArray = Encoding.UTF8.GetBytes(
            "<html><head><title>CubeTools - Authenticated</title></head>" +
            "<body>You have been authenticated. Please go back to Cube Tools.</body></html><script type=\"text/javascript\">window.close() ;</script>");
            byte[] _errorArray = Encoding.UTF8.GetBytes(
                "<html><head><title>CubeTools - Error</title></head>"+
                "<body>An error occured in authentication. Please try again later.</body></html>");
            if (param.Get("code") != null)
            {
                context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
                context.Response.KeepAlive = false;
                context.Response.Close();
                HttpClient client = new HttpClient();
                var values = new Dictionary<string, string>
                {
                    {"grant_type", "authorization_code"},
                    {"code", param.Get("code")},
                    {"client_id", _clientId},
                    {"redirect_uri", _redirectUri}
                };
                var content = new FormUrlEncodedContent(values);
                Task<HttpResponseMessage> response =
                    client.PostAsync(_tokenUri, content);
                response.Wait();
                Task<string> resStr = response.Result.Content.ReadAsStringAsync();
                resStr.Wait();
                token = JsonConvert.DeserializeObject<Token>(resStr.Result);
                authenticated?.Invoke(this, true);
            }
            else
            {
                context.Response.OutputStream.Write(_errorArray, 0, _errorArray.Length);
                context.Response.KeepAlive = false;
                context.Response.Close();
                Console.WriteLine("Error in authentication");
                authenticated?.Invoke(this, false);
            }
        }
    }
}