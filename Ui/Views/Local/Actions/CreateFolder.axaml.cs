using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;

namespace Ui.Views.Local.Actions
{
    public class CreateFolder : Window
    {
        private readonly Local _main;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreateFolder()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
        }
        public CreateFolder(Local main) : this()
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
            if (!ManagerReader.IsPathCorrect(name))
                _textEntered.Text = "Invalid Text !";
            else if (Directory.Exists(name))
                _main.SelectErrorPopUp(new ReplaceException("File already exists !"));
            else
            {
                try
                {
                    _main.NavigationBarView.FolderPointer.AddDir(name);
                    _main.PathsBarView.ReloadPath(_main.NavigationBarView.FolderPointer.ChildrenFiles);
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