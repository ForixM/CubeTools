using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class ActionBar : UserControl
    {
        public static ActionBarViewModel ViewModel;

        #region Init
        public ActionBar()
        {
            InitializeComponent();
            ViewModel = new ActionBarViewModel(this);
            DataContext = ViewModel;
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
        private void CreateFile(object? sender, RoutedEventArgs e)
        {
            try
            {
                var ft = ManagerWriter.Create();
                ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items =
                    ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer
                        .ChildrenFiles);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to create a new file";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, @managerException).Show();
                }
            }
        }

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        private void CreatDir(object? sender, RoutedEventArgs e)
        {
            try
            {
                var ft = ManagerWriter.CreateDir();
                ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items =
                    ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer
                        .ChildrenFiles);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to create a new directory";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, @managerException).Show();
                }
            }
        }

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        private void Copy(object? sender, RoutedEventArgs e)
        {
            ViewModel.CopiedXaml.Clear();
            ViewModel.CutXaml.Clear();
            foreach (var item in ViewModel.SelectedXaml)
                ViewModel.CopiedXaml.Add(item);
        }

        /// <summary>
        /// Add to cut the selected elements
        /// </summary>
        private void Cut(object? sender, RoutedEventArgs e)
        {
            ViewModel.CopiedXaml.Clear();
            ViewModel.CutXaml.Clear();
            foreach (var item in ViewModel.SelectedXaml)
            {
                ViewModel.CopiedXaml.Add(item);
                ViewModel.CutXaml.Add(item);
            }
        }

        /// <summary>
        /// Paste copied and delete cut
        /// </summary>
        private void Paste(object? sender, RoutedEventArgs e)
        {
            // 1) Copy Copied
            foreach (var item in ViewModel.CopiedXaml)
                CopyPointer((FileType) item.DataContext!);
            
            // Reload Path
            ViewModel.ParentViewModel!.ReloadPath();
            
            // 2) Destroy Cut
            foreach (var item in ViewModel.CutXaml)
                DeletePointer((FileType) item.DataContext!);

            // Reload Path
            ViewModel.ParentViewModel!.ReloadPath();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        private void Rename(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedXaml.Count >= 1)
            {
                if (ViewModel.SelectedXaml.Count == 1)
                    new RenamePopUp((FileType) ViewModel.SelectedXaml[0].DataContext!, ViewModel.ParentViewModel.ViewModelPathsBar.Items!, ViewModel.ParentViewModel).Show();
                else
                    new ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel,
                        new ManagerException("Unable to rename multiple data")).Show();
            }
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        private void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in ViewModel.SelectedXaml)
                DeletePointer((FileType) item.DataContext!);
            ViewModel?.ParentViewModel?.ReloadPath();
        }

        /// <summary>
        /// Display the search box
        /// </summary>
        private void Search(object? sender, RoutedEventArgs e)
        {
            var searchPopup = new SearchPopUp(ViewModel);
            searchPopup.Show();
        }

        /// <summary>
        /// Sort the ListBox
        /// </summary>
        private void Sort(object? sender, RoutedEventArgs e)
        {
            ManagerReader.SortByName(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            ViewModel.ParentViewModel.ViewModelPathsBar.Items =
                ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer
                    .ChildrenFiles);
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
            var task = new Task(() => ManagerWriter.Copy(source.Path));
            // Calculate the size of the pointer
            int nbFiles = ManagerReader.FastReaderFiles(source.Path);
            // Getting new Path
            string path = ManagerReader.GenerateNameForModification(source.Path).Replace('\\','/');
            // Remove reference from Directory Pointer
            ViewModel.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.Remove(source);
            // Run Tasks Async
            try
            {
                if (source.Size > 1000000 && source.IsDir)
                {
                    // Run async task
                    task.Start();
                    // Display loading box
                    var loadingPopUp = new LoadingPopUp(true);
                    loadingPopUp.Show();
                    // Update percentages
                    int current = ManagerReader.FastReaderFiles(path);
                    while (current != nbFiles && Directory.Exists(source.Path))
                    {
                        current = ManagerReader.FastReaderFiles(source.Path);
                        loadingPopUp.ReloadProgress(current, nbFiles);
                        Thread.Sleep(200);
                    }
                    // Close display
                    loadingPopUp.Close();
                }
                else task.RunSynchronously();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to copy {source.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, @managerException).Show();
                }
            }

            FileType copied = new FileType(path);
            ViewModel.ParentViewModel!.ViewModelNavigationBar.DirectoryPointer.AddChild(copied);
        }

        /// <summary>
        /// Delete the given pointer
        /// </summary>
        /// <param name="source">The given pointer to delete</param>
        private void DeletePointer(FileType source)
        {
            // Create a new task to delete the pointer
            var task = new Task(() => ManagerWriter.DeleteDir(source));
            // Calculate the size of the pointer
            int nbFiles = ManagerReader.FastReaderFiles(source.Path);
            // Unable to remove files
            int unableRemove = 0;
            // Remove reference from Directory Pointer
            ViewModel.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.Remove(source);
            // Run Tasks Async
            try
            {
                if (source.Size > 1000000 && source.IsDir)
                {
                    // Run async task
                    task.Start();
                    // Display loading box
                    var loadingPopUp = new LoadingPopUp(true);
                    loadingPopUp.Show();
                    // Update percentages
                    int current = ManagerReader.FastReaderFiles(source.Path);
                    int last = 0;
                    int infiniteLoopAvoider = 10;
                    while (current-unableRemove > 0 && infiniteLoopAvoider != 0 && Directory.Exists(source.Path))
                    {
                        last = current;
                        current = ManagerReader.FastReaderFiles(source.Path);
                        if (last != current)
                            infiniteLoopAvoider = 10;
                        else 
                            infiniteLoopAvoider -= 1;
                        loadingPopUp.ReloadProgress(current, nbFiles);
                        Thread.Sleep(250);
                    }
                    // Close display
                    loadingPopUp.Close();
                }
                // Run task sync
                else
                    task.RunSynchronously();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    unableRemove--;
                    @managerException.Errorstd = $"Unable to delete {source.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, @managerException).Show();
                }
            }
        }
        
        #endregion
    }
}
