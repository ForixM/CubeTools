using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.Views.Settings;

namespace CubeTools_UI.Views
{
    public class MainWindow : Window
    {
        public MainWindowModel Model;
        
        public MainWindow()
        {
            InitializeComponent();
            Model = new MainWindowModel(this);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    Model.IsCtrlPressed = true;
                    break;
                case Key.Delete :
                    Model.ModelLocal.ModelActionBar.View.Delete(sender, e);
                    break;
                case Key.Enter :
                    if (Model.ModelLocal.ModelActionBar.SelectedXaml.Count != 0) 
                        Model.ModelLocal.AccessPath(Model.ModelLocal.ModelActionBar.SelectedXaml[0].Pointer.Path);
                    break;
                case Key.F2 :
                    Model.ModelLocal.ModelActionBar.View.Rename(sender, e);
                    break;
                default:
                    if (Model.IsCtrlPressed)
                    {
                        switch (e.Key)
                        {
                            case Key.W:
                                Close();
                                break;
                            case Key.N:
                                new MainWindow().Show();
                                break;
                            case Key.C:
                                Model.ModelLocal.ModelActionBar.View.Copy(sender, e);
                                break;
                            case Key.X:
                                Model.ModelLocal.ModelActionBar.View.Cut(sender, e);
                                break;
                            case Key.V:
                                Model.ModelLocal.ModelActionBar.View.Paste(sender, e);
                                break;
                            case Key.A:
                                // All items are selected
                                if (Model.ModelLocal.ModelActionBar.SelectedXaml.Count ==
                                    Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count)
                                {
                                    Model.ModelLocal.ModelActionBar.SelectedXaml.Clear();
                                    int size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                                    for (int i = 0; i < size; i++)
                                    {
                                        Model.ModelLocal.ModelActionBar.SelectedXaml.Add((PointerItem) Model.ModelLocal.ModelPathsBar.View.Generator.Children[i]);
                                        size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                                    }
                                }
                                else
                                {
                                    int size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                                    for (int i = 0; i < size; i++)
                                    {
                                        if (!Model.ModelLocal.ModelActionBar.SelectedXaml.Contains((PointerItem)Model.ModelLocal.ModelPathsBar.View.Generator.Children[i]))
                                            Model.ModelLocal.ModelActionBar.SelectedXaml.Add((PointerItem) Model.ModelLocal.ModelPathsBar.View.Generator.Children[i]);
                                        size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                                    }
                                }
                                Model.ModelLocal.ReloadPath();
                                break;
                            case Key.F:
                                Model.ModelLocal.ModelActionBar.View.Search(sender, e);
                                break;
                            case Key.H:
                            case Key.R:
                                Model.ModelLocal.ModelNavigationBar.View.SyncClick(sender, e);
                                break;
                            case Key.I:
                                new SettingsWindow().Show();
                                break;
                        }
                    }
                    break;
            }
        }

        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.LeftCtrl or Key.RightCtrl)
                Model.IsCtrlPressed = false;
        }
        
    }
}