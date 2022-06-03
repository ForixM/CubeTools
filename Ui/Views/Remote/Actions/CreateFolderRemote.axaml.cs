using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;

namespace Ui.Views.Remote.Actions
{
    public class CreateFolderRemote : Window
    {
        private readonly MainWindowRemote _main;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreateFolderRemote()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
        }
        public CreateFolderRemote(MainWindowRemote main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
                CreateDir(_textEntered.Text);
        }

        private void OnPressed(object? sender, RoutedEventArgs e) => CreateDir(_textEntered.Text);
        
        private void OnCancelPressed(object? sender, RoutedEventArgs e) => Close(true);
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    Close();
                    break;
                case Key.Enter:
                    CreateDir(_textEntered.Text);
                    break;
            }
        }
        
        #endregion

        #region Process

        private void CreateDir(string name)
        {
            if (_main.Client is null) return;
            else if (!ManagerReader.IsPathCorrect(name)) _textEntered.Text = "Invalid Text !";
            else if (_main.Client.GetItem(name) is not null)
                _main.SelectErrorPopUp(new ReplaceException("Folder already exists !"));
            else
            {
                try
                {
                    _main.Client.CreateFolder(name);
                    _main.ReloadPath();
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new folder";
                        _main.SelectErrorPopUp(@managerException);
                    }
                }
            }
        }

        #endregion

        
    }
}