using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views.Ftp
{
    public class LocalAction : UserControl
    {
        public MainWindowFTP ParentView;
        
        #region Init

        public LocalAction()
        {
            InitializeComponent();
            ParentView = null;
        }

        public LocalAction(MainWindowFTP main)
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
        }

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        public void CreatDir(object? sender, RoutedEventArgs e)
        {
            try
            {
                var ft = ManagerWriter.CreateDir();
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
        }

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        public void Copy(object? sender, RoutedEventArgs e)
        {
            ParentView.Local.FtpModel.Copied.Clear();
            ParentView.Local.FtpModel.Cut.Clear();
            foreach (var item in ParentView.Local.FtpModel.Selected)
                ParentView.Local.FtpModel.Copied.Add(item);
        }

        /// <summary>
        /// Add to cut the selected elements
        /// </summary>
        public void Cut(object? sender, RoutedEventArgs e)
        {
            ParentView.Local.FtpModel.Copied.Clear();
            ParentView.Local.FtpModel.Cut.Clear();
            foreach (var item in ParentView.Local.FtpModel.Selected)
            {
                ParentView.Local.FtpModel.Copied.Add(item);
                ParentView.Local.FtpModel.Cut.Add(item);
            }
        }

        /// <summary>
        /// Paste copied and delete cut
        /// </summary>
        public void Paste(object? sender, RoutedEventArgs e)
        {
            // 1) Copy Copied
            foreach (var item in ParentView.Local.FtpModel.Copied)
                CopyPointer(item.Pointer);

            // 2) Destroy Cut
            foreach (var item in ParentView.Local.FtpModel.Cut)
                DeletePointer(item.Pointer);
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        public void Rename(object? sender, RoutedEventArgs e)
        {
            if (ParentView.Local.FtpModel.Selected.Count < 1) return;
            
            /*
            if (ParentView.Local.FtpModel.Selected.Count == 1)
                //new RenamePopUp(ParentView.Local.FtpModel.Selected[0].Pointer, ParentView.Local.FtpModel.LocalDirectory.ChildrenFiles, ParentView.Model).Show();
            else
                new ErrorPopUp.ErrorPopUp(Model.ParentModel, new ManagerException("Unable to rename multiple data")).Show();
            */
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        public void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in ParentView.Local.FtpModel.Selected)
                DeletePointer(item.Pointer);
        }

        /// <summary>
        /// Display the search box
        /// </summary>
        public void Search(object? sender, RoutedEventArgs e)
        {
            //var searchPopup = new SearchPopUp(Model);
            //searchPopup.Show();
        }
        
        private void Upload(object? sender, RoutedEventArgs e)
        {
            foreach (var item in ParentView.Local.FtpModel.Selected.Where(item => !item.Pointer.IsDir))
                ParentView.Client!.UploadFile(item.Pointer, ParentView.Remote.FtpModel.RemoteDirectory);
            foreach (var item in ParentView.Local.FtpModel.Selected.Where(item => item.Pointer.IsDir))
                ParentView.Client!.UploadFolder(item.Pointer, ParentView.Remote.FtpModel.RemoteDirectory);
        }
        
        
        #endregion

        #region Process
     
        /// <summary>
        /// Copy the given pointer
        /// </summary>
        /// <param name="source">The pointer that we want to make a copy</param>
        private void CopyPointer(FileType source)
        {
            // Create a new task to delete the pointer
            var task = new Task<FileType>(() =>
            {
                if (ParentView.Local.FtpModel.LocalDirectory.Path == ManagerReader.GetParent(source.Path).Replace('\\','/'))
                    return ManagerWriter.Copy(source.Path);
                return ManagerWriter.Copy(source.Path,
                        ParentView.Local.FtpModel.LocalDirectory.Path + "/" + source.Name);
            });
            // Remove reference from Directory Pointer
            ParentView.Local.FtpModel.LocalDirectory.Remove(source);
            // Run Tasks Async
            try
            {
                if (source.Size > 1000000)
                {
                    // Run async task
                    task.Start();
                    // Display loading box
                    var loadingPopUp = new LoadingPopUp((int) ManagerReader.GetFileSize(source), source);
                    loadingPopUp.Show();
                    // Close display
                    task.GetAwaiter().OnCompleted(() =>
                    {
                        loadingPopUp.Close();
                        try
                        {
                            ParentView.Local.FtpModel.LocalDirectory.AddChild(task.Result);
                        }
                        catch (Exception e)
                        {
                            /*
                            if (e is ManagerException @managerException)
                                new ErrorPopUp.ErrorPopUp(Model.ParentModel, managerException).Show();
                            */
                        }
                        ParentView.ReloadPathLocal();
                    });
                }
                else
                {
                    task.RunSynchronously();
                    ParentView.Local.FtpModel.LocalDirectory.AddChild(task.Result);
                    ParentView.ReloadPathLocal();
                }
            }
            catch (Exception exception)
            {
                /*
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to copy {source.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(Model.ParentModel, @managerException).Show();
                }
                */
            }
        }
        
        private void DeletePointer(FileType pointer)
        {
            // Create a new task to delete the pointer
            var task = new Task(() =>
            {
                if (pointer.IsDir)
                    ManagerWriter.DeleteDir(pointer);
                else 
                    ManagerWriter.Delete(pointer);
            });
            // Remove reference from Directory Pointer
            ParentView.Local.FtpModel.LocalDirectory.Remove(pointer);
            // Run Tasks Async
            try
            {
                if (pointer.Size > 1000000)
                {
                    // Run async task
                    task.Start();
                    // Display loading box
                    var loadingPopUp = new LoadingPopUp((int) ManagerReader.GetFileSize(pointer), pointer,true);
                    loadingPopUp.Show();
                    // Close display
                    task.GetAwaiter().OnCompleted(() =>
                    {
                        loadingPopUp.Close();
                        ParentView.ReloadPathLocal();
                    });
                }
                // Run task sync
                else
                {
                    task.RunSynchronously();
                    ParentView.ReloadPathLocal();
                }
            }
            catch (Exception exception)
            {
                /*
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to delete {pointer.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(Model!, @managerException).Show();
                }
                */
            }
        }

        #endregion

        
    }
}
