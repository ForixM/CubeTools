using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Onedrive
{
    public class OnedriveClient
    {
        private HttpListener _listener;
        
        private static string _clientId = "044195b6-72dd-47b6-b904-c2e84618919a";
        private static string _redirectUri = "http://localhost:4040";

        private Token token;
        private string path = "/";

        private static string _authorityUri =
            string.Format(
                "https://login.live.com/oauth20_authorize.srf?client_id={0}&scope=onedrive.readwrite%20offline_access&response_type=code&redirect_uri={1}",
                Uri.EscapeDataString(_clientId),
                Uri.EscapeDataString(_redirectUri));

        private static string _tokenUri = "https://login.live.com/oauth20_token.srf";
        private static string _api = "https://api.onedrive.com/v1.0/drive/root:/";
        
        public OnedriveClient()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(_redirectUri+"/");
            _listener.Start();
            Thread thread = new Thread(Authenticator);
            thread.Start();
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

        private async void Authenticator()
        {
            HttpListenerContext context = _listener.GetContext(); // get a context
            Uri uri = context.Request.Url;
            NameValueCollection param = HttpUtility.ParseQueryString(uri.Query);
            // Now, you'll find the request URL in context.Request.Url
            byte[] _responseArray = Encoding.UTF8.GetBytes("<html><head><title>CubeTools - Authenticated</title></head>" + 
                                                           "<body>You have been authenticated. Please go back to Cube Tools.</body></html>"); // get the bytes to response
            context.Response.OutputStream.Write(_responseArray, 0, _responseArray.Length); // write bytes to the output stream
            context.Response.KeepAlive = false; // set the KeepAlive bool to false
            context.Response.Close(); // close the connection
            Console.WriteLine("Respone given to a request.");
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
                string onedrive = string.Format("https://api.onedrive.com/v1.0/drive/root:/test.txt?access_token={0}&select=name,size,createdDateTime",
                    Uri.EscapeDataString(token.access_token));
                Task<string> responseString = client.GetStringAsync(_api+":/children?access_token="+token.access_token+"&select=name,size,folder,file");
                responseString.Wait();
                var arbo = JsonConvert.DeserializeObject<OneArboresence>(responseString.Result);
                Console.WriteLine(arbo.ToString());
            }
            else
            {
                Console.WriteLine("Error");
            }
        }
    }
}