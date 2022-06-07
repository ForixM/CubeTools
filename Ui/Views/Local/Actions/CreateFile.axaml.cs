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
    public class CreateFile : Window
    {
        private readonly Local? _main;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreateFile()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
            _main = null;
        }
        public CreateFile(Local main) : this()
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
            if (_main == null) return;
            
            if (!ManagerReader.IsPathCorrect(name))
            {
                new ErrorBase(new PathFormatException("Format is invalid !", "Create a local file")).ShowDialog<object>(_main.Main);
            }
            else if (File.Exists(_main.NavigationBarView.FolderLocalPointer.Path + "/"+name))
            {
                new ErrorBase(new ReplaceException("File already exists !", "Create a local file")).ShowDialog<bool>(_main.Main);
            }
            else
            {
                try
                {
                    string[] split = name.Split(".");
                    switch (split.Length)
                    {
                        case 0:
                            _main.NavigationBarView.FolderLocalPointer.AddFile("New File", "txt");
                            break;
                        case 1:
                            _main.NavigationBarView.FolderLocalPointer.AddFile(name, "");
                            break;
                        default:
                        {
                            string nameNoExtension = "";
                            for (int i = 0; i < split.Length - 1; i++) nameNoExtension += split[i];
                            _main.NavigationBarView.FolderLocalPointer.AddFile(nameNoExtension, split[-1]);
                            break;
                        }
                    }
                    _main.PathsBarView.Refresh();
                    Close(null);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new file";
                        new ErrorBase(@managerException).ShowDialog<object>(_main.Main);
                    }
                }
            }
        }

        #endregion
    }
}