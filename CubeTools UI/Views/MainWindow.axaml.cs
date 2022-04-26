using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;

namespace CubeTools_UI.Views
{
    public class MainWindow : Window
    {
        public MainWindowViewModel ViewModel;
        public MainWindow()
        {
            InitializeComponent();
            this.AttachDevTools();
            ViewModel = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
             if (e.Key is Key.LeftCtrl or Key.RightCtrl)
                ViewModel.IsCtrlPressed = true;
             else if (ViewModel.IsCtrlPressed)
             {
                 switch (e.Key)
                 {
                     case Key.W :
                         Close();
                         break;
                     case Key.N :
                         new MainWindow().Show();
                         break;
                     case Key.C :
                         ViewModel.ViewModelActionBar.AttachedView.Copy(sender, e);
                         break;
                     case Key.X :
                         ViewModel.ViewModelActionBar.AttachedView.Cut(sender, e);
                         break;
                     case Key.V :
                         ViewModel.ViewModelActionBar.AttachedView.Paste(sender, e);
                         break;
                     case Key.A :
                         ViewModel.ViewModelActionBar.SelectedXaml.Clear();
                         int size = ViewModel.ViewModelPathsBar.AttachedView.Generator.Children.Count;
                         for (int i = 0; i < size; i++)
                         {
                             ViewModel.ViewModelActionBar.SelectedXaml.Add((PointerItem)ViewModel.ViewModelPathsBar.AttachedView.Generator.Children[i]);
                             size = ViewModel.ViewModelPathsBar.AttachedView.Generator.Children.Count;
                         }
                         ViewModel.ReloadPath();
                         break;
                     case Key.F :
                         ViewModel.ViewModelActionBar.AttachedView.Search(sender, e);
                         break;
                     case Key.H:
                     case Key.R :
                         ViewModel.ViewModelNavigationBar.AttachedView.SyncClick(sender, e);
                         break;
                 }
             }
        }

        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.LeftCtrl or Key.RightCtrl)
                ViewModel.IsCtrlPressed = false;
        }
    }
}