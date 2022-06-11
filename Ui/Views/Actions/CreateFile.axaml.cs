using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.Error;

namespace Ui.Views.Actions
{
    public class CreateFile : Window
    {
        private readonly ClientUI? _main;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreateFile()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
            _main = null;
        }
        public CreateFile(ClientUI main) : this()
        {
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter) CreateAFile(_textEntered.Text);
        }
        private void OnPressed(object? sender, RoutedEventArgs e) => CreateAFile(_textEntered.Text);
        private void OnCancelPressed(object? sender, RoutedEventArgs e) => Close();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) CreateAFile(_textEntered.Text);
        }
        
        #endregion

        #region Process

        /// <summary>
        /// Create a file with a given name
        /// </summary>
        /// <param name="name">The given name</param>
        private void CreateAFile(string name)
        {
            if (_main?.Client?.CurrentFolder is null) return;
            else if (!ManagerReader.IsPathCorrect(name))
            {
                new ErrorBase(new PathFormatException("Format is invalid !", "CreateAFile UI")).Show();
                //new ErrorBase(new PathFormatException("Format is invalid !", "CreateAFile UI")).ShowDialog<bool>(_main.Main);
            }
            else if (_main.Client.GetItem(name) is not null)
            {
                new ErrorBase(new ReplaceException("File already exists !", "CreateAFile UI")).Show();
                //new ErrorBase(new ReplaceException("File already exists !", "CreateAFile UI")).ShowDialog<bool>(_main.Main);
            }
            else
            {
                try
                {
                    _main.Client.CreateFile(string.IsNullOrEmpty(name) ? "New File.txt" : name);
                    _main.Refresh();
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new file";
                        new ErrorBase(managerException).Show();
                        //new ErrorBase(managerException).ShowDialog<object>(_main.Main);
                    }
                }
            }
        }

        #endregion

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main.Main is MainWindow window)
            {
                window.KeysPressed.Remove(e.Key);
            }
            else if (_main.Main is MainWindowRemote windowRemote)
            {
                windowRemote.KeysPressed.Remove(e.Key);
            };
        }
    }
}