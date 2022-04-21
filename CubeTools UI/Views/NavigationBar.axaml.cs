using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.ManagerReader;

namespace CubeTools_UI.Views
{
    public class NavigationBar : UserControl
    {
        public static NavigationBarViewModel ViewModel;
        public TextBox CurrentPathXaml;
        
        public NavigationBar()
        {
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("CurrentPath");
            ViewModel = new NavigationBarViewModel(this);
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            ViewModel.ParentViewModel?.AccessPath(((TextBox) sender).Text);
        }

        private void LeftArrowClick(object? sender, RoutedEventArgs e)
        {
            // End of the queue
            if (ViewModel.QueueIndex > 0)
            {
                // Get the index before
                ViewModel.QueueIndex--;
                try
                {
                    string path = ViewModel.QueuePointers[ViewModel.QueueIndex];
                    ViewModel.ParentViewModel?.AccessPath(path);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                        ViewModel.ParentViewModel?.ErrorMessageBox(@managerException, $"Unable to get the last directory");
                    ViewModel.QueueIndex--;
                }
            }
        }

        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            if (ViewModel.QueueIndex < ViewModel.QueuePointers.Count - 1)
            {
                ViewModel.QueueIndex++;
                try
                {
                    ViewModel.ParentViewModel?.AccessPath(ViewModel.QueuePointers[ViewModel.QueueIndex]);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                        ViewModel.ParentViewModel?.ErrorMessageBox(@managerException, $"Unable to get the next directory");
                    ViewModel.QueueIndex--;
                }
            }
        }

        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            string parent = "";
            try
            {
                if (ManagerReader.GetRootPath(ViewModel.DirectoryPointer.Path) ==
                    ViewModel.DirectoryPointer.Path)
                    parent = ViewModel.DirectoryPointer.Path;
                else 
                    parent = ManagerReader.GetParent(ViewModel.DirectoryPointer.Path);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                    ViewModel.ParentViewModel?.ErrorMessageBox(@managerException, $"Unable to get directory of ");
                else
                    throw;
            }
            
            ViewModel.QueuePointers.Add(parent);
            ViewModel.QueueIndex = ViewModel.QueuePointers.Count-1;
            try
            {
                ViewModel.ParentViewModel?.AccessPath(ViewModel.QueuePointers[ViewModel.QueueIndex]);
            }
            catch (Exception exception)
            {
                if ( exception is ManagerException @managerException)
                    ViewModel.ParentViewModel?.ErrorMessageBox(@managerException, "Unable to get parent");
            }
        }

        private void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                ViewModel.DirectoryPointer.SetChildrenFiles();
                ViewModel.ParentViewModel.ViewModelPathsBarXaml.AttachedView.ItemsXaml.Items = ManagerReader.ListToObservable(ViewModel.ParentViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                    ViewModel.ParentViewModel?.ErrorMessageBox(@managerException, "Enable to reload the directory");
            }
        }

        private void SettingsClick(object? sender, RoutedEventArgs e)
        {
            // TODO Implement a settings Window
        }
    }
}
