using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class PathsBar : UserControl
    {
        public static PathsBarViewModel? ViewModel;
        public ListBox ItemsXaml;
        public PathsBar()
        {
            InitializeComponent();
            ItemsXaml = this.FindControl<ListBox>("ItemsXaml");
            ViewModel = new PathsBarViewModel(this);
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        /// <summary>
        /// XAML Method : On Item double taped
        /// </summary>
        private void OnDoubleTaped(object? sender, RoutedEventArgs e)
        {
            
            if (((Button) sender!)?.DataContext is FileType context && ViewModel?.ModelXaml != null)
            {
                ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Clear();
                ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Add(context);
                ViewModel.ParentViewModelXaml.AccessPath(context.Path, Directory.Exists(context.Path));
            }
        }

        /// <summary>
        /// XAML Method : On Item taped
        /// </summary>
        private void OnTaped(object? sender, PointerPressedEventArgs e)
        {
            ViewModel.ParentViewModelXaml.ErrorMessageBox(new AccessException());
            Button button = ((Button) sender!);
            if (button.DataContext is FileType context && ViewModel?.ModelXaml != null)
            {
                if (File.Exists(context.Path) || Directory.Exists(context.Path))
                {
                    //ListBox? listBox = (ListBox) ((ListBoxItem) button.Parent!).Parent!;
                    button.Background = new SolidColorBrush(new Color(255, 200, 200, 200));
                    if (e.MouseButton is MouseButton.Left)
                    {
                        ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Clear();
                        ViewModel.ModelXaml.ModelActionBar.SelectedXaml.Add(context);
                    }
                    else if (e.MouseButton is MouseButton.Right)
                    {
                        ViewModel.ParentViewModelXaml.PropertiesBox(context);
                    }
                }
                else
                {
                    ViewModel.ParentViewModelXaml.ErrorMessageBox(new PathNotFoundException($"{context.Path} does not exist anymore"));
                    ViewModel.ParentViewModelXaml.ReloadPath();
                }
            }
        }
    }
}
