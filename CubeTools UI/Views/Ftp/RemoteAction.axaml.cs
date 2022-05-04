using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;
using LibraryFTP;

namespace CubeTools_UI.Views.Ftp
{
    public class RemoteAction : UserControl
    {
        public MainWindowFTP ParentView;
        
        #region Init

        public RemoteAction()
        {
            InitializeComponent();
            ParentView = null;
        }

        public RemoteAction(MainWindowFTP main)
        {
            ParentView = main;
        }
            
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion
        
        #region Events

        /// <summary>
        /// Create a new file (New File by default)
        /// </summary>
        public void CreateFile(object? sender, RoutedEventArgs e)
        {
            /*
            try
            {
                var ft = ManagerWriter.Create();
                ParentView.Local.FtpModel.LocalDirectory.ChildrenFiles.Add(ft);
                ParentView.ReloadPathLocal();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException)
                {
                    //@managerException.Errorstd = "Unable to create a new file";
                    //new Views.ErrorPopUp.ErrorPopUp(_model!, @managerException).Show();
                }
            }
            */
        }

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        public void CreatDir(object? sender, RoutedEventArgs e)
        {
            ParentView.Client!.MakeDirectory(ParentView.Remote.FtpModel.RemoteDirectory, "New Folder");
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        public void Copy(object? sender, RoutedEventArgs e)
        {
            ParentView.Remote.FtpModel.Copied.Clear();
            ParentView.Remote.FtpModel.Cut.Clear();
            foreach (var item in ParentView.Remote.FtpModel.Selected)
                ParentView.Remote.FtpModel.Copied.Add(item);
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Add to cut the selected elements
        /// </summary>
        public void Cut(object? sender, RoutedEventArgs e)
        {
            ParentView.Remote.FtpModel.Copied.Clear();
            ParentView.Remote.FtpModel.Cut.Clear();
            foreach (var item in ParentView.Remote.FtpModel.Selected)
            {
                ParentView.Remote.FtpModel.Copied.Add(item);
                ParentView.Remote.FtpModel.Cut.Add(item);
            }
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Paste copied and delete cut
        /// </summary>
        public void Paste(object? sender, RoutedEventArgs e)
        {
            // 1) Copy Copied
            foreach (var item in ParentView.Remote.FtpModel.Copied)
                CopyPointer(item.Pointer);

            // 2) Destroy Cut
            foreach (var item in ParentView.Remote.FtpModel.Cut)
                DeletePointer(item.Pointer);
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        public void Rename(object? sender, RoutedEventArgs e)
        {
            if (ParentView.Remote.FtpModel.Selected.Count < 1) return;
            
            /*
            if (ParentView.Local.FtpModel.Selected.Count == 1)
                //new RenamePopUp(ParentView.Local.FtpModel.Selected[0].Pointer, ParentView.Local.FtpModel.LocalDirectory.ChildrenFiles, ParentView.Model).Show();
            else
                new ErrorPopUp.ErrorPopUp(Model.ParentModel, new ManagerException("Unable to rename multiple data")).Show();
            */
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        public void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in ParentView.Remote.FtpModel.Selected)
                DeletePointer(item.Pointer);
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Display the search box
        /// </summary>
        public void Search(object? sender, RoutedEventArgs e)
        {
            //var searchPopup = new SearchPopUp(Model);
            //searchPopup.Show();
            ParentView.ReloadPathRemote();
        }

        /// <summary>
        /// Display the sort box
        /// </summary>
        public void Sort(object? sender, RoutedEventArgs e)
        {
            //var popup = new SortPopUp(Model.ParentModel);
            //popup.Show();
            ParentView.ReloadPathRemote();
        }
        
        
        #endregion

        #region Process
     
        /// <summary>
        /// Copy the given pointer
        /// </summary>
        /// <param name="pointer">The pointer that we want to make a copy</param>
        private void CopyPointer(IFtpItem pointer)
        {
            // TODO Mehdi : Implement High Level functionalities for FTP => Copy async
        }
        
        private void DeletePointer(IFtpItem pointer)
        {
            // TODO Mehdi : Implement High level functionalities for FTP => Delete async
            ParentView.Client!.DeleteItem(pointer);
        }

        #endregion

        private void Parent(object? sender, RoutedEventArgs e)
        {
            ParentView.Remote.FtpModel.RemoteDirectory =
                new FtpFolder(ManagerReader.GetParent(ParentView.Local.FtpModel.LocalDirectory.Path));
            ParentView.ReloadPathRemote();
        }
    }
}
