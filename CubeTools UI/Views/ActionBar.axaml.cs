using System;
using System.Collections.Generic;
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
                    ViewModel.ParentViewModel.ErrorMessageBox(@managerException, "Unable to create a new file");
                else throw;
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
                    ViewModel.ParentViewModel.ErrorMessageBox(@managerException, "Unable to create a new file");
                else throw;
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
        /// 
        /// </summary>
        private void Paste(object? sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in ViewModel.CopiedXaml)
                    ViewModel.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.AddChild(ManagerWriter.Copy(((FileType) item.DataContext).Path));
                foreach (var item in ViewModel.CutXaml)
                {
                    var ft = (FileType) item.DataContext!;
                    ManagerWriter.Delete(ft);
                    ViewModel.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.Remove(ft);
                }
                ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                    ViewModel?.ParentViewModel?.ErrorMessageBox(@managerException, "Unable to copy");
                else throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rename(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedXaml.Count >= 1)
            {
                if (ViewModel.SelectedXaml.Count == 1)
                {
                    var renamePopup = new RenamePopUp((FileType) ViewModel.SelectedXaml[0].DataContext, ViewModel?.ParentViewModel?.ViewModelPathsBar.Items);
                    renamePopup.Show();
                }
                else
                {
                    ViewModel?.ParentViewModel?.ErrorMessageBox(new ManagerException(), "Unable to rename multiple data");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete(object? sender, RoutedEventArgs e)
        {
            List<string> undestroyed = new List<string>();
            ManagerException lastError = new ManagerException();
            foreach (var item in ViewModel.SelectedXaml)
            {
                var ft = (FileType) item.DataContext!;
                try
                {
                    ManagerWriter.Delete(ft);
                    ViewModel?.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.Remove(ft);
                    ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
                }
                catch (Exception exception)
                {
                    if (exception is CorruptedPointerException or CorruptedDirectoryException)
                    {
                        ViewModel?.ParentViewModel?.ViewModelNavigationBar.DirectoryPointer.Remove(ft);
                        ViewModel.ParentViewModel.ViewModelPathsBar.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
                    }
                    else if (exception is ManagerException @managerException)
                    {
                        undestroyed.Add(ft.Path);
                        lastError = managerException;
                    }
                }
            }

            if (undestroyed.Count != 0)
            {
                string res = "";
                foreach (var s in undestroyed)
                    res += s + ",";
                ViewModel?.ParentViewModel?.ErrorMessageBox(lastError, "CubeTools was unable to destroy : " +res);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Search(object? sender, RoutedEventArgs e)
        {
            var searchPopup = new SearchPopUp(ViewModel);
            searchPopup.Show();
        }

        private void Sort(object? sender, RoutedEventArgs e)
        {
            ManagerReader.SortByName(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            ViewModel.ParentViewModel.ViewModelPathsBar.Items =
                ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer
                    .ChildrenFiles);
        }
    }
}
