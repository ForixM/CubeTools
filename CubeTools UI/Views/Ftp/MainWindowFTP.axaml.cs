using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models.Ftp;
using Library.Pointers;
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
        public RemoteFtpNavigationBar NavigationBar;

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
            this.FindControl<LocalAction>("LocalAction").ParentView = this;
            this.FindControl<RemoteAction>("RemoteAction").ParentView = this;
            NavigationBar = this.FindControl<RemoteFtpNavigationBar>("RemoteNavigationBar");
            NavigationBar.ParentView = this;
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
            if (isdir)
            {
                Local.FtpModel.LocalDirectory = new DirectoryType(path);
                ReloadPathLocal();
            }
            else
            {
                // TODO Open the file
            }
            
        }

        #endregion

        #region Children

        public void ReloadPathRemote()
        {
            // Reload Pointers
            Remote.FtpModel.Children = Client!.ListDirectory(Remote.FtpModel.RemoteDirectory);
            // Reload Graphic
            Remote.ReloadPath(Remote.FtpModel.Children.Items);
            NavigationBar.CurrentPathXaml.Text = Remote.FtpModel.RemoteDirectory.Path;
        }

        public void AccessPathRemote(IFtpItem item, bool isdir)
        {
            if (isdir)
            {
                Remote.FtpModel.RemoteDirectory = (FtpFolder)item;
                //Remote.FtpModel.Children = Model.Client.ListDirectory((FtpFolder)item);
                ReloadPathRemote();
            }
            else
            {
                Model.Client.DownloadFile((FtpFile)item, Local.FtpModel.LocalDirectory.Path);
                ReloadPathLocal();
            }
        }

        #endregion
    }
}