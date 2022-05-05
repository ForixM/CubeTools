using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;

namespace CubeTools_UI.Views.ErrorPopUp
{
    public class AccessDeniedPopUp : Window
    {
        public readonly TextBlock ContentError;
        private readonly Button ButtonYes;
        private readonly Button ButtonNo;
        
        public readonly MainWindowModel? ParentModel;
        public AccessDeniedPopUp()
        {
            InitializeComponent();
            ContentError = this.FindControl<TextBlock>("ContentError");
            ButtonYes = this.FindControl<Button>("ButtonYes");
            ButtonNo = this.FindControl<Button>("ButtonNo");
        }
        public AccessDeniedPopUp(MainWindowModel parent) : this()
        {
            ParentModel = parent;
        }
        public AccessDeniedPopUp(MainWindowModel parent,AccessException exception) : this(parent)
        {
            ContentError.Text = exception.ErrorMessage;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        // EVENTS
        
        private void ButtonYesClicked(object? sender, RoutedEventArgs e)
        {
            // TODO Edit Yes button for admin action
        }

        private void ButtonNoClicked(object? sender, RoutedEventArgs e) => Close();

        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}