using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using LibraryFTP;

namespace CubeTools_UI.Views.Ftp
{
    public class MainWindowFTP : Window
    {

        public ClientFtp? Client;
        
        #region Init
        
        public MainWindowFTP()
        {
            InitializeComponent();
            Client = null;
            Title = "CubeTools FTP -";
        }

        public MainWindowFTP(ClientFtp client) : this()
        { 
            Client = client;
            Title += client.Host;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion
    }
}