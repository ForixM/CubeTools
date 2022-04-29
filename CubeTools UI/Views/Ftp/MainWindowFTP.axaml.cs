using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models.Ftp;
using LibraryFTP;

namespace CubeTools_UI.Views.Ftp
{
    public class MainWindowFTP : Window
    {
        #region Variables
        
        public ClientFtp? Client;
        public MainWindowFTPModel Model;
        public LocalFTP Local;
        public RemoteFTP Remote;
        
        #endregion

        #region Init
        
        public MainWindowFTP()
        {
            InitializeComponent();
            // Global
            Model = null;
            Client = null;
            Title = "CubeTools FTP -";
            Local = null;
        }

        public MainWindowFTP(ClientFtp client) : this()
        { 
            Client = client;
            Model = new MainWindowFTPModel(this, Client);
            Title += client.Host;
            Remote = new RemoteFTP(this);
            Local = new LocalFTP(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Local

        public void ReloadPathLocal()
        {
            //
        }

        public void AccessPathLocal(string path, bool isdir)
        {
            
        }

        #endregion

        #region Remote

        public void ReloadPathRemote()
        {
            
        }

        public void AccessPathRemote(string path, bool isdir)
        {
            
        }

        #endregion
    }
}