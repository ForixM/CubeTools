using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;

namespace CubeTools_UI.Views
{
    public class MainWindow : Window
    {
        public MainWindowModel Model;
        public MainWindow()
        {
            InitializeComponent();
            //this.AttachDevTools();
            Model = new MainWindowModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
             if (e.Key is Key.LeftCtrl or Key.RightCtrl)
                Model.IsCtrlPressed = true;
             else if (Model.IsCtrlPressed)
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
                         Model.ModelActionBar.View.Copy(sender, e);
                         break;
                     case Key.X :
                         Model.ModelActionBar.View.Cut(sender, e);
                         break;
                     case Key.V :
                         Model.ModelActionBar.View.Paste(sender, e);
                         break;
                     case Key.A :
                         Model.ModelActionBar.SelectedXaml.Clear();
                         int size = Model.ModelPathsBar.View.Generator.Children.Count;
                         for (int i = 0; i < size; i++)
                         {
                             Model.ModelActionBar.SelectedXaml.Add((PointerItem)Model.ModelPathsBar.View.Generator.Children[i]);
                             size = Model.ModelPathsBar.View.Generator.Children.Count;
                         }
                         Model.ReloadPath();
                         break;
                     case Key.F :
                         Model.ModelActionBar.View.Search(sender, e);
                         break;
                     case Key.H:
                     case Key.R :
                         Model.ModelNavigationBar.View.SyncClick(sender, e);
                         break;
                 }
             }
        }

        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.LeftCtrl or Key.RightCtrl)
                Model.IsCtrlPressed = false;
        }
    }
}