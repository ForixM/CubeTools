using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.Error;

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
            {
                new ErrorBase(new PathFormatException($"{name} is an invalid name", "Create a local folder"))
                    .ShowDialog<bool>(_main.Main);
            }
            else if (Directory.Exists(name))
            {
                new ErrorBase(new ReplaceException("File already exists !", "Create a local folder")).ShowDialog<bool>(_main.Main);
            }
            else
            {
                try
                {
                    _main.NavigationBarView.FolderLocalPointer.AddDir(name);
                    _main.PathsBarView.Refresh(_main.NavigationBarView.FolderLocalPointer.ChildrenFiles);
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new folder";
                        new ErrorBase(@managerException).ShowDialog<object>(_main.Main);
                    }
                }
            }
        }

        #endregion

        
    }
}