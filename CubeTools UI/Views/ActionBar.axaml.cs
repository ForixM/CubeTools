using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class ActionBar : UserControl
    {
        public static ActionBarModel Model;

        #region Init
        public ActionBar()
        {
            InitializeComponent();
            Model = new ActionBarModel(this);
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
            new CreatePopUp(Model.ParentModel).Show();
        }

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        public void CreatDir(object? sender, RoutedEventArgs e)
        {
            new CreateFolderPopUp(Model.ParentModel).Show();
        }

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        public void Copy(object? sender, RoutedEventArgs e)
        {
            Model.CopiedXaml.Clear();
            Model.CutXaml.Clear();
            foreach (var item in Model.SelectedXaml)
                Model.CopiedXaml.Add(item);
        }

        /// <summary>
        /// Add to cut the selected elements
        /// </summary>
        public void Cut(object? sender, RoutedEventArgs e)
        {
            Model.CopiedXaml.Clear();
            Model.CutXaml.Clear();
            foreach (var item in Model.SelectedXaml)
            {
                Model.CopiedXaml.Add(item);
                Model.CutXaml.Add(item);
            }
        }

        /// <summary>
        /// Paste copied and delete cut
        /// </summary>
        public void Paste(object? sender, RoutedEventArgs e)
        {
            // 1) Copy Copied
            foreach (var item in Model.CopiedXaml)
                CopyPointer(item.Pointer);

            // 2) Destroy Cut
            foreach (var item in Model.CutXaml)
                new DeletePopUp(Model.ParentModel, item.Pointer).Show();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        public void Rename(object? sender, RoutedEventArgs e)
        {
            if (Model.SelectedXaml.Count < 1) return;
            
            if (Model.SelectedXaml.Count == 1)
                new RenamePopUp(Model.SelectedXaml[0].Pointer, Model.ParentModel.ModelNavigationBar.DirectoryPointer.ChildrenFiles, Model.ParentModel).Show();
            else
                new ErrorPopUp.ErrorPopUp(Model.ParentModel, new ManagerException("Unable to rename multiple data")).Show();
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        public void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in Model.SelectedXaml)
                new DeletePopUp(Model.ParentModel, item.Pointer).Show();
            Model.ParentModel.ReloadPath();
        }

        /// <summary>
        /// Display the search box
        /// </summary>
        public void Search(object? sender, RoutedEventArgs e)
        {
            var searchPopup = new SearchPopUp(Model);
            searchPopup.Show();
        }

        /// <summary>
        /// Display the sort box
        /// </summary>
        public void Sort(object? sender, RoutedEventArgs e)
        {
            var popup = new SortPopUp(Model.ParentModel);
            popup.Show();
        }
        
        /// <summary>
        /// Open the Snap Drop application in a browser
        /// </summary>
        private void Snap(object? sender, RoutedEventArgs e)
        {
            var uri = "https://www.snapdrop.net";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }
        
        /// <summary>
        /// Open the smash application in a browser
        /// </summary>
        private void Smash(object? sender, RoutedEventArgs e)
        {
            const string uri = "https://www.fromsmash.com";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = uri
            };
            System.Diagnostics.Process.Start(psi);
        }

        /// <summary>
        /// Compress selected files adn folders by displaying folder and files popup for compression
        /// </summary>
        private void Compress(object? sender, RoutedEventArgs e)
        {
            if (Model.SelectedXaml.Count == 0) return;
            
            var folder = Model.SelectedXaml.Where(ft => ft.Pointer.IsDir).Select(item => item.Pointer);
            var files = Model.SelectedXaml.Where(ft => !ft.Pointer.IsDir).Select(item => item.Pointer);

            var fileTypes = files.ToList();
            if (fileTypes.Any())
                new CompressPopUp(Model.ParentModel, fileTypes).Show();
            
            var folderTypes = folder.ToList();
            if (folderTypes.Any())
                foreach (var dir in folderTypes) 
                    new CompressFolderPopUp(Model.ParentModel, dir).Show();   
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
                if (Model.ParentModel.ModelNavigationBar.DirectoryPointer.Path == ManagerReader.GetParent(source.Path).Replace('\\','/'))
                    return ManagerWriter.Copy(source.Path);
                return ManagerWriter.Copy(source.Path,
                        Model.ParentModel.ModelNavigationBar.DirectoryPointer.Path + "/" + source.Name);
            });
            // Remove reference from Directory Pointer
            Model.ParentModel.ModelNavigationBar.DirectoryPointer.Remove(source);
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
                            Model.ParentModel.ModelNavigationBar.DirectoryPointer.AddChild(task.Result);
                        }
                        catch (Exception e)
                        {
                            if (e is ManagerException @managerException)
                                new ErrorPopUp.ErrorPopUp(Model.ParentModel, managerException).Show();
                        }
                        Model.ParentModel.ReloadPath();
                    });
                }
                else
                {
                    task.RunSynchronously();
                    Model.ParentModel.ModelNavigationBar.DirectoryPointer.AddChild(task.Result);
                    Model.ParentModel.ReloadPath();
                }
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to copy {source.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(Model.ParentModel, @managerException).Show();
                }
            }
        }

        #endregion
    }
}
