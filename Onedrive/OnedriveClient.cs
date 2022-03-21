using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Manager;
using Manager.Pointers;
using MimeTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Onedrive
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
        private string path = "/drive/root:";
        private HttpClient _client;

        private static string _authorityUri =
            string.Format(
                "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=onedrive.readwrite%20offline_access&response_type=code&redirect_uri={1}",
                Uri.EscapeDataString(_clientId),
                Uri.EscapeDataString(_redirectUri));

        private static string _tokenUri = "https://login.live.com/oauth20_token.srf";
        private static string _api = "https://api.onedrive.com/v1.0";

        public delegate void SampleEventHandler(object sender, bool success);

        public delegate void UploadUpdateHandler(object sender, int percent);

        public event SampleEventHandler authenticated;
        public event SampleEventHandler uploadFinished;
        public event SampleEventHandler downloadFinished;
        // public event UploadUpdateHandler uploadUpdate;

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
                client.GetStringAsync(_api + path + ":/children?access_token=" + token.access_token +
                                      "&select=name,size,folder,file,parentReference,id");
            responseString.Wait();
            try
            {
                return JsonConvert.DeserializeObject<OneArboresence>(responseString.Result);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        
        public OneArboresence GetArboresence(OneItem folder)
        {
            if (folder.Type != OneItemType.FOLDER) return null;
            HttpClient client = new HttpClient();
            Task<string> responseString =
                client.GetStringAsync(_api + folder.path + ":/children?access_token=" + token.access_token +
                                      "&select=name,size,folder,file,parentReference,id");
            responseString.Wait();
            try
            {
                return JsonConvert.DeserializeObject<OneArboresence>(responseString.Result);
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
                client.PostAsync(_api + path + ":/children?access_token=" + token.access_token,
                    new StringContent(data, Encoding.UTF8, "application/json"));
            response.Wait();
            Task<string> resStr = response.Result.Content.ReadAsStringAsync();
            resStr.Wait();
            return (int) response.Result.StatusCode == 201;
        }
        
        public bool CreateFolder(string name, OneItem folder)
        {
            if (folder.Type != OneItemType.FOLDER)
                return false;
            string data = "{'name':'"+name+"','folder':{ },'@microsoft.graph.conflictBehavior':'rename'}";
            Task<HttpResponseMessage> response =
                _client.PostAsync(
                    _api + folder.parentReference.path + "/" + folder.name + ":/children?access_token=" +
                    token.access_token,
                    new StringContent(data, Encoding.UTF8, "application/json"));
            response.Wait();
            return (int) response.Result.StatusCode == 201;
        }


        public async Task<bool> UploadFile(FileType file)
        {
            HttpResponseMessage response = await _client.PutAsync(
                _api + path + "/" + Path.GetFileName(file.Path) + ":/content?access_token=" + token.access_token,
                new StringContent(System.IO.File.ReadAllText(file.Path), Encoding.UTF8,
                    MimeTypeMap.GetMimeType(file.Path)));
            uploadFinished?.Invoke(this, (int)response.StatusCode == 201);
            return (int) response.StatusCode == 201;
        }

        public FileType DownloadFile(string dest, OneItem item)
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + item.parentReference.path + "/" + item.name + ":/content?access_token=" +
                                 token.access_token);
            response.Wait();
            Task<byte[]> resStr = response.Result.Content.ReadAsByteArrayAsync();
            resStr.Wait();
            File.WriteAllBytes(dest, resStr.Result);
            downloadFinished?.Invoke(this, true); //TODO handle success bool value
            return new FileType(dest);
        }
        
        public FileType DownloadFile(string dest, string path)
        {
            if (path.StartsWith("./"))
            {
                path = path.Remove(0, 2);
                path = this.path + path;
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
            return new FileType(dest);
        }

        public bool DeleteItem(OneItem item)
        {
            Task<HttpResponseMessage> response =
                _client.DeleteAsync(_api + item.parentReference.path + "/" + item.name + "?access_token=" +
                                    token.access_token);
            response.Wait();
            return (int) response.Result.StatusCode == 204;
        }
        
        public bool DeleteItem(string path)
        {
            Task<HttpResponseMessage> response =
                _client.DeleteAsync(_api + this.path + path + "?access_token=" + token.access_token);
            response.Wait();
            return (int) response.Result.StatusCode == 204;
        }

        public bool MoveItem(OneItem item, OneItem destination)
        {
            if (destination.Type != OneItemType.FOLDER) return false;
            JObject body = new JObject();
            body.Add(new JProperty("parentReference", new JObject(new JProperty("id", destination.id))));
            body.Add(new JProperty("name", item.name));
            Task<HttpResponseMessage> response =
                _client.PatchAsync(
                    _api + item.parentReference.path + "/" + item.name + "?access_token=" + token.access_token,
                    new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            response.Wait();
            return (int) response.Result.StatusCode == 200;
        }

        public JObject GetItemFullMetadata(OneItem item)
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + item.path + "?access_token=" + token.access_token);
            response.Wait();
            Task<string> strResponse = response.Result.Content.ReadAsStringAsync();
            strResponse.Wait();
            return (int)response.Result.StatusCode==200 ? JObject.Parse(strResponse.Result) : null;
        }

        public JObject CreateShareLink(OneItem item, SharePermission permission) //TODO Not working
        {
            throw new NotImplementedException();
            // JObject body = new JObject();
            // body.Add(new JProperty("type", "view"));
            // body.Add(new JProperty("scope", "anonymous"));
            // Console.WriteLine(item.path);
            // Console.WriteLine(_api + item.path + ":/createLink?access_token=oui");
            // Console.WriteLine(body.ToString());
            // Task<HttpResponseMessage> response =
            //     _client.PostAsync(_api + item.path + ":/createLink?access_token=" + token.access_token,
            //         new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            // // Task<HttpResponseMessage> response =
            // //     _client.PostAsync(_api + item.path + ":/createLink?access_token=" + token.access_token,
            // //         new StringContent(body.ToString(), Encoding.UTF8, "application/json"));
            // response.Wait();
            // Task<string> strResponse = response.Result.Content.ReadAsStringAsync();
            // strResponse.Wait();
            // // Console.WriteLine((int)response.Result.StatusCode);
            // Console.WriteLine("oui: "+strResponse.Result);
            // JObject data = GetItemFullMetadata(item);
            // string link = data.GetValue("webUrl").ToString();
            // return JObject.Parse("{'url':'"+link+"'}");
            // return (int)response.Result.StatusCode is 200 or 201 ? JObject.Parse(strResponse.Result) : null;
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

        public JObject GetPermissions(OneItem item)
        {
            Task<HttpResponseMessage> response =
                _client.GetAsync(_api + item.path + ":/permissions?access_token=" + token.access_token);
            response.Wait();
            Task<string> str = response.Result.Content.ReadAsStringAsync();
            str.Wait();
            return JObject.Parse(str.Result);
        }

        public bool Copy(OneItem item, OneItem newPath)
        {
            if (newPath.Type != OneItemType.FOLDER) return false;
            JObject body = new JObject();
            JObject parentReference = new JObject();
            parentReference.Add("driveId", item.parentReference.driveId);
            parentReference.Add("id", newPath.id);
            body.Add("parentReference", parentReference);
            body.Add("name", "test.docx");
            Console.WriteLine(body);
            Task<HttpResponseMessage> response =
                _client.PostAsync(_api + item.path + ":/copy?access_token="+token.access_token,
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
            // byte[] _responseArray = Encoding.UTF8.GetBytes(
            //     "<html><head><title>CubeTools - Authenticated</title></head>" +
            //     "<body>You have been authenticated. Please go back to Cube Tools.</body></html><script>window.onbeforeunload = function (e) {e = e || window.event;if (e) {e.returnValue = 'Sure?';}return 'Sure?';};</script>");
            context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length);
            context.Response.KeepAlive = false;
            context.Response.Close();
            // Console.WriteLine("Respone given to a request.");
            if (param.Get("code") != null)
            {
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
            }
            else
            {
                Console.WriteLine("Error in authentication");
                authenticated?.Invoke(this, false);
            }
            authenticated?.Invoke(this, true);
        }
    }
}