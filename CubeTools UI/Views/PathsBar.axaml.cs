using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.Pointers;
using MouseButton = Avalonia.Remote.Protocol.Input.MouseButton;

namespace CubeTools_UI.Views
{
    public class PathsBar : UserControl
    {
        public static PathsBarViewModel ViewModel;
        public PathsBar()
        {
            InitializeComponent();
            ViewModel = new PathsBarViewModel();
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnClicked(object? sender, RoutedEventArgs e)
        {
            if (((Button) e.Source!)?.DataContext is FileType context)
            {
                if (ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Count > 0 && ViewModel.ModelXaml.ModelActionBar.SelectedXaml[0].Path == context.Path)
                {
                    try
                    {
                        ViewModel.ParentViewModelXaml.AccessPath(context.Path, Directory.Exists(context.Path));
                    }
                    catch (Exception exception)
                    {
                        if (exception is ManagerException @managerException)
                            ViewModel.ParentViewModelXaml.ErrorMessageBox(@managerException, $"Error while accessing {context.Path}");
                    }
                }
                else
                {
                    ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Clear();
                    ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Add(context);
                }
            }
        }
    }
}
