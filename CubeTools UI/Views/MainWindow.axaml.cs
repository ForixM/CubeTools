using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.Views.ErrorPopUp;
using CubeTools_UI.Views.Settings;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views
{
    public class MainWindow : Window
    {
        public MainWindowModel Model;
        public MainWindow()
        {
            InitializeComponent();
            Model = new MainWindowModel();
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
                    Model.ModelActionBar.View.Delete(sender, e);
                    break;
                case Key.Enter :
                    if (Model.ModelActionBar.SelectedXaml.Count != 0) 
                        Model.AccessPath(Model.ModelActionBar.SelectedXaml[0].Pointer.Path);
                    break;
                case Key.F2 :
                    Model.ModelActionBar.View.Rename(sender, e);
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
                                Model.ModelActionBar.View.Copy(sender, e);
                                break;
                            case Key.X:
                                Model.ModelActionBar.View.Cut(sender, e);
                                break;
                            case Key.V:
                                Model.ModelActionBar.View.Paste(sender, e);
                                break;
                            case Key.A:
                                // All items are selected
                                if (Model.ModelActionBar.SelectedXaml.Count ==
                                    Model.ModelPathsBar.View.Generator.Children.Count)
                                {
                                    Model.ModelActionBar.SelectedXaml.Clear();
                                    int size = Model.ModelPathsBar.View.Generator.Children.Count;
                                    for (int i = 0; i < size; i++)
                                    {
                                        Model.ModelActionBar.SelectedXaml.Add((PointerItem) Model.ModelPathsBar.View.Generator.Children[i]);
                                        size = Model.ModelPathsBar.View.Generator.Children.Count;
                                    }
                                }
                                else
                                {
                                    int size = Model.ModelPathsBar.View.Generator.Children.Count;
                                    for (int i = 0; i < size; i++)
                                    {
                                        if (!Model.ModelActionBar.SelectedXaml.Contains((PointerItem)Model.ModelPathsBar.View.Generator.Children[i]))
                                            Model.ModelActionBar.SelectedXaml.Add((PointerItem) Model.ModelPathsBar.View.Generator.Children[i]);
                                        size = Model.ModelPathsBar.View.Generator.Children.Count;
                                    }
                                }
                                Model.ReloadPath();
                                break;
                            case Key.F:
                                Model.ModelActionBar.View.Search(sender, e);
                                break;
                            case Key.H:
                            case Key.R:
                                Model.ModelNavigationBar.View.SyncClick(sender, e);
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


        public void SelectErrorPopUp(ManagerException exception)
        {
            switch (exception)
            {
                case PathNotFoundException @pathNotFoundException:
                    new PathNotFoundPopUp(Model, @pathNotFoundException).Show();
                    Close();
                    break;
                case AccessException @accessException:
                    new AccessDeniedPopUp(Model, @accessException).Show();
                    Close();
                    break;
                case DiskNotReadyException @diskNotReadyException:
                    new DiskNotReadyPopUp(Model, @diskNotReadyException).Show();
                    Close();
                    break;
                case SystemErrorException @systemErrorException:
                    new SystemErrorPopUp(Model, @systemErrorException).Show();
                    Close();
                    break;
                default:
                    new ErrorPopUp.ErrorPopUp(Model, exception).Show();
                    break;
            }
        }
    }
}