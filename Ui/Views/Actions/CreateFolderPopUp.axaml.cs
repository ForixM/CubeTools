using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Ui.Models;

namespace Ui.Views.Actions
{
    public class CreateFolderPopUp : Window
    {
        private readonly LocalModel _model;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreateFolderPopUp()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
        }
        public CreateFolderPopUp(LocalModel vm) : this()
        {
            _model = vm;
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
                _model.SelectErrorPopUp(new ReplaceException("File already exists !"));
            else
            {
                try
                {
                    var ft = ManagerWriter.CreateDir(name);
                    _model.ModelNavigationBar.FolderPointer.ChildrenFiles.Add(ft);
                    _model.ModelPathsBar.ReloadPath(_model.ModelNavigationBar.FolderPointer
                        .ChildrenFiles);
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new folder";
                        _model.SelectErrorPopUp(@managerException);
                    }
                }
            }
        }

        #endregion

        
    }
}