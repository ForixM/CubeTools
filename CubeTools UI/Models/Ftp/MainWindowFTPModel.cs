using CubeTools_UI.Views.Ftp;
using LibraryFTP;

namespace CubeTools_UI.Models.Ftp
{
    public class MainWindowFTPModel
    {
        private ClientFtp _client;
        public ClientFtp Client => _client;

        public MainWindowFTP View;

        public MainWindowFTPModel(MainWindowFTP view, ClientFtp client)
        {
            _client = client;
            View = view;
        }
    }
}