using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using LibraryClient;
using Ui.Views.Remote.Actions;

namespace Ui.Views.Remote
{
    public class RemoteAction : UserControl
    {
        public MainWindowRemote? Main;

        public List<RemoteItem> Selected;
        public List<RemoteItem> Copied;
        public List<RemoteItem> CutXaml;

        #region Init

        public RemoteAction()
        {
            InitializeComponent();
            Main = null;
            Selected = new List<RemoteItem>();
            Copied = new List<RemoteItem>();
            CutXaml = new List<RemoteItem>();
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
        public void CreateFile(object? sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        public void CreatDir(object? sender, RoutedEventArgs e) => new CreateFileRemote(Main!).Show();

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        public void Copy(object? sender, RoutedEventArgs e)
        {
            Copied.Clear();
            CutXaml.Clear();
            foreach (var item in Main!.RemoteActionView.Selected) Copied.Add(item);
            Main.ReloadPath();
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
            Main!.ReloadPath();
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
            
            Main!.ReloadPath();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        public void Rename(object? sender, RoutedEventArgs e)
        {
            if (Selected.Count == 1)
                new RenameRemote(Main!.RemoteActionView.Selected[0], Main.Client.Children, Main).Show();
            else new ErrorPopUp.ErrorPopUp(new ManagerException("Unable to rename multiple data")).Show();
            
            Main!.ReloadPath();
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        public void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in Main!.RemoteActionView.Selected)
                DeletePointer(item);
            Main.ReloadPath();
        }


        #endregion

        #region Process
     
        /// <summary>
        /// Copy the given pointer
        /// </summary>
        /// <param name="pointer">The pointer that we want to make a copy</param>
        private void CopyPointer(RemoteItem pointer)
        {
            // TODO Mehdi : Implement High Level functionalities for FTP => Copy async
            Main!.Client.Copy(pointer);
        }
        
        private void DeletePointer(RemoteItem pointer)
        {
            // TODO Mehdi : Implement High level functionalities for FTP => Delete async
            Main!.Client.Delete(pointer);
        }

        #endregion
    }
}
