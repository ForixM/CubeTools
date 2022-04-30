using System.IO;
using Avalonia;
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
            Title = "CubeTools FTP -";
            Local = this.FindControl<LocalFTP>("LocalFtp");
            Remote = this.FindControl<RemoteFTP>("RemoteFtp");
        }

        public MainWindowFTP(ClientFtp client) : this()
        { 
            Client = client;
            Model = new MainWindowFTPModel(this, Client);
            Title += client.Host;
            // Local
            Local.FtpModel = new LocalFTPModel(Model, Local, Directory.GetCurrentDirectory());
            ReloadPathLocal();
            // Remote
            Remote.FtpModel = new RemoteFTPModel(Model, Remote, "/");
            ReloadPathRemote();
            // Debug
            this.AttachDevTools();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region LocalDirectory

        public void ReloadPathLocal()
        {
            // Reload Pointers
            Local.FtpModel.LocalDirectory.SetChildrenFiles();
            // Reload Graphic
            Local.ReloadPath(Local.FtpModel.LocalDirectory.ChildrenFiles);
        }

        public void AccessPathLocal(string path, bool isdir)
        {
        }

        #endregion

        #region Children

        public void ReloadPathRemote()
        {
            // Reload Pointers
            Remote.FtpModel.Children = Client!.ListDirectory(Remote.FtpModel.RemoteDirectory);
            // Reload Graphic
            Remote.ReloadPath(Remote.FtpModel.Children.Items);
        }

        public void AccessPathRemote(string path, bool isdir)
        {
            
        }

        #endregion
    }
}