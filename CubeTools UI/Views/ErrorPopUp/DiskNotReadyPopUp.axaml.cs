using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views.ErrorPopUp
{
    public class DiskNotReadyPopUp : Window
    {
        
        #region Attached Components
        
        public readonly TextBlock ContentError;
        private readonly Button ButtonQuit;
        private readonly Button ButtonReload;
        
        #endregion

        public readonly MainWindowModel? ParentModel;

        public DiskNotReadyPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
            ButtonQuit = this.FindControl<Button>("ButtonQuit");
            ButtonReload = this.FindControl<Button>("ButtonReload");
        }
        public DiskNotReadyPopUp(MainWindowModel parent) : this()
        {
            ParentModel = parent;
        }
        public DiskNotReadyPopUp(MainWindowModel parent,DiskNotReadyException exception) : this(parent)
        {
            ContentError.Text = exception.ErrorMessage;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);


        // EVENTS

        private void ButtonCancelClicked(object? sender, RoutedEventArgs e) => Close();

        private void ButtonReloadClicked(object? sender, RoutedEventArgs e)
        {
            ParentModel!.ReloadPath();
            Close();
        }

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}