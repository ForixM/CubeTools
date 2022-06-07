using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library;
using Ui.Views.Error;
using Ui.Views.Remote.Actions;

namespace Ui.Views.Remote
{
    public class RemoteAction : UserControl
    {
        public MainWindowRemote? Main;

        public List<Pointer> Selected;
        public List<Pointer> Copied;
        public List<Pointer> CutXaml;

        #region Init

        public RemoteAction()
        {
            Main = MainWindowRemote.LastReference;
            InitializeComponent();
            Selected = new List<Pointer>();
            Copied = new List<Pointer>();
            CutXaml = new List<Pointer>();
        }

        public RemoteAction(MainWindowRemote main) : this()
        {
            Main = main;
        }
            
        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events

        /// <summary>
        /// Create a new file (New File by default)
        /// </summary>
        public void CreateFile(object? sender, RoutedEventArgs e) => new CreateFileRemote(Main!).Show();

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        public void CreatDir(object? sender, RoutedEventArgs e) => new CreateFolderRemote(Main!).Show();

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        public void Copy(object? sender, RoutedEventArgs e)
        {
            Copied.Clear();
            CutXaml.Clear();
            foreach (var item in Main!.RemoteActionView.Selected) Copied.Add(item);
            Main.Refresh();
        }

        /// <summary>
        /// Add to cut the selected elements
        /// </summary>
        public void Cut(object? sender, RoutedEventArgs e)
        {
            Copied.Clear();
            CutXaml.Clear();
            foreach (var item in Selected)
            {
                Copied.Add(item);
                CutXaml.Add(item);
            }
            Main!.Refresh();
        }

        /// <summary>
        /// Paste copied and delete cut
        /// </summary>
        public void Paste(object? sender, RoutedEventArgs e)
        {
            // 1) Copy Copied
            foreach (var item in Copied)
                Main!.Client.Copy(item);

            // 2) Destroy Cut
            foreach (var item in CutXaml)
                Main!.Client.Delete(item);
            
            Main!.Refresh();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        public void Rename(object? sender, RoutedEventArgs e)
        {
            switch (Selected.Count)
            {
                case 0:
                    return;
                case 1:
                    new RenameRemote(Main!.RemoteActionView.Selected[0], Main.Client.Children, Main).Show();
                    break;
                default:
                    new ErrorBase(new ManagerException("Unable to rename multiple data")).ShowDialog<bool>(Main);
                    break;
            }

            Main!.Refresh();
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        public void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in Main!.RemoteActionView.Selected)
                DeletePointer(item);
            Main.Refresh();
        }
        
        /// <summary>
        /// Delete selected
        /// </summary>
        public void Download(object? sender, RoutedEventArgs e)
        {
            if (Main?.Client?.CurrentFolder is null) return;

            foreach (var item in Main!.RemoteActionView.Selected)
            {
                if (item.IsDir) Main.Client.DownloadFolder(item, Main.Local.NavigationBarView.FolderPointer);
                else Main.Client.DownloadFile(item, Main.Local.NavigationBarView.FolderPointer);
            }
            Main.Refresh();
        }


        #endregion

        #region Process
     
        /// <summary>
        /// Copy the given pointer
        /// </summary>
        /// <param name="pointer">The pointer that we want to make a copy</param>
        private void CopyPointer(Pointer pointer)
        {
            // TODO Mehdi : Implement High Level functionalities for FTP => Copy async
            Main!.Client.Copy(pointer);
        }
        
        private void DeletePointer(Pointer pointer)
        {
            // TODO Mehdi : Implement High level functionalities for FTP => Delete async
            Main!.Client.Delete(pointer);
        }

        #endregion
    }
}
