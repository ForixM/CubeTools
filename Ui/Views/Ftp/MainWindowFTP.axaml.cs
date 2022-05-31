using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library;
using LibraryFTP;
using Ui.Models.Ftp;

namespace Ui.Views.Ftp
{
    public class MainWindowFTP : Window
    {
        #region Variables
        
        public ClientFtp? Client;
        
        public MainWindowFTPModel Model;
        
        public Local Local;
        
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
            Local = this.FindControl<Local>("Local");
            Remote = this.FindControl<RemoteFTP>("RemoteFtp");
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
            ReloadPathLocal();
            // Remote
            Remote.FtpModel = new RemoteFTPModel(Model, Remote, "/");
            if (ReloadPathRemote())
            {
                NavigationBar.Model.Add(Remote.FtpModel.RemoteDirectory);
                Show();
            }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region LocalDirectory

        public void ReloadPathLocal()
        {
            // Reload Pointers
            Local.Model.ModelNavigationBar.FolderPointer.SetChildrenFiles();
            // Reload Graphic
            Local.Model.ReloadPath();
        }

        public void AccessPathLocal(string path, bool isdir)
        {
            if (isdir)
            {
                Local.Model.ModelNavigationBar.FolderPointer = new DirectoryPointer(path);
                ReloadPathLocal();
            }
            else
            {
                Local.Model.AccessPath(path, isdir);
            }
            
        }

        #endregion

        #region Children

        public bool ReloadPathRemote()
        {
            try
            {
                // Reload Pointers
                Remote.FtpModel.Children = Client!.ListDirectory(Remote.FtpModel.RemoteDirectory);
                // Reload Graphic
                Remote.ReloadPath(Remote.FtpModel.Children.Items);
                NavigationBar.CurrentPathXaml.Text = Remote.FtpModel.RemoteDirectory.Path;
                return true;
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                    new Views.ErrorPopUp.ErrorPopUp();
                return false;
            }
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
                Model.Client.DownloadFile((FtpFile)item, Local.Model.ModelNavigationBar.FolderPointer.Path);
                ReloadPathLocal();
            }
        }

        #endregion
    }
}