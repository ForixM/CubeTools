using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;
using ReactiveUI;

namespace CubeTools_UI.Views
{
    public class ActionBar : UserControl
    {
        public static ActionBarViewModel ViewModel;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateFile(object? sender, RoutedEventArgs e)
        {
            try
            {
                var ft = ManagerWriter.Create();
                ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
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
        /// 
        /// </summary>
        private void CreatDir(object? sender, RoutedEventArgs e)
        {
            try
            {
                var ft = ManagerWriter.CreateDir();
                ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
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
        /// Copy 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Copy(object? sender, RoutedEventArgs e)
        {
            ViewModel.CopiedXaml.Clear();
            ViewModel.CutXaml.Clear();
            foreach (var item in ViewModel.SelectedXaml) 
                ViewModel.CopiedXaml.Add(item);
        }

        /// <summary>
        /// 
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
            {
                var ft = (FileType) item.DataContext;
                var task = new Task(() => CopyAsync(ft));
                if (ft.Size > 1000000) task.Start();
                else task.RunSynchronously();
            }
            // 2) Destroy Cut
            foreach (var item in ViewModel.CutXaml)
            {
                var ft = (FileType) item.DataContext;
                var task = new Task(() => DeleteAsync(ft));
                if (ft.Size > 1000000) task.Start();
                else task.RunSynchronously();
            }
            ViewModel.ParentViewModel.ReloadPath();
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        private void Rename(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedXaml.Count >= 1)
            {
                if (ViewModel.SelectedXaml.Count == 1)
                    new RenamePopUp((FileType) ViewModel.SelectedXaml[0].DataContext, ViewModel?.ParentViewModel?.ViewModelPathsBar.Items).Show();
                else 
                    new ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, new ManagerException("Unable to rename multiple data")).Show();
            }
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        private void Delete(object? sender, RoutedEventArgs e)
        {
            foreach (var item in ViewModel.SelectedXaml)
            {
                var ft = (FileType) item.DataContext;
                var task = new Task(() => DeleteAsync(ft));
                if (ft.Size > 1000000) task.Start();
                else task.RunSynchronously();
                ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            }
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
        
        
        // PROCESS
        private void CopyAsync(FileType item)
        {
            try
            {
                var ft = ManagerWriter.Copy(item.Path);
                ViewModel.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.AddChild(ft);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to copy {item.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, @managerException).Show();
                }
            }
        }

        private void DeleteAsync(FileType ft)
        {
            ViewModel.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.Remove(ft);
            try
            {
                if (ft.IsDir)
                    ManagerWriter.DeleteDir(ft);
                else 
                    ManagerWriter.Delete(ft);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to delete {ft.Name}";
                    new Views.ErrorPopUp.ErrorPopUp(ViewModel.ParentViewModel, @managerException).Show();
                }
            }
        }
    }
}
