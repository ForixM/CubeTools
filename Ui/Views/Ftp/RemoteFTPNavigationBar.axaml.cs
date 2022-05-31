using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using LibraryFTP;
using Ui.Models;

namespace Ui.Views.Ftp
{
    public class RemoteFtpNavigationBar : UserControl
    {
        // public static NavigationBarModel Model;
        public FTPNavigationBarModel Model;
        public TextBox CurrentPathXaml;
        public MainWindowFTP ParentView;
        
        #region Init
        public RemoteFtpNavigationBar()
        {
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("RemoteCurrentPath");
            Model = new FTPNavigationBarModel();
            //Model = new NavigationBarModel(this);
            //DataContext = Model;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion
        
        #region Events
        
        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            FtpFolder folder = new FtpFolder(((TextBox) sender!).Text);
            ParentView.Remote.FtpModel.RemoteDirectory = folder;
            ParentView.ReloadPathRemote();
            // Model.ParentModel?.AccessPath(((TextBox) sender!).Text);
        }

        /// <summary>
        /// The last pointer is chosen
        /// </summary>
        private void LeftArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Model.Index > 0)
            {
                Model.Index--;
                ParentView.AccessPathRemote(Model.Queue[Model.Index], true);
            }
        }

        /// <summary>
        /// The next pointer in the stack is chosen
        /// </summary>
        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Model.Index < Model.Queue.Count - 1)
            {
                Model.Index++;
                ParentView.AccessPathRemote(Model.Queue[Model.Index], true);
            }
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            if (ParentView.Remote.FtpModel.RemoteDirectory != FtpFolder.ROOT)
            {
                ParentView.Remote.FtpModel.RemoteDirectory =
                    (FtpFolder) ParentView.Remote.FtpModel.RemoteDirectory.Parent;
                if (ParentView.Remote.FtpModel.RemoteDirectory is { } folder && ParentView.Model != null)
                    ((MainWindowFTP)ParentView.Model.View).NavigationBar.Model.Add(folder);
                // ParentView.Remote.FtpModel.Children = ParentView.Model.Client.ListDirectory((FtpFolder) ParentView.Remote.FtpModel.RemoteDirectory.Parent);
                ParentView.ReloadPathRemote();
            }
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // Model.FolderPointer.SetChildrenFiles();
                // Model.ParentModel!.ReloadPath();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to reload file";
                    // new Views.ErrorPopUp.ErrorPopUp(Model.ParentModel!, @managerException).Show();
                }
            }
        }
        
        /// <summary>
        /// The settings is opened
        /// </summary>
        private void SettingsClick(object? sender, RoutedEventArgs e)
        {
            // TODO Implement a settings Window
        }
        
        #endregion
    }
}
