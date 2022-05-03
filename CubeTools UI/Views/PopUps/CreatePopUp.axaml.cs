using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;

namespace CubeTools_UI.Views.PopUps
{
    public class CreatePopUp : Window
    {
        private readonly MainWindowModel? _model;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreatePopUp()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
            _model = null;
        }
        public CreatePopUp(MainWindowModel vm) : this()
        {
            _model = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion
        
        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                CreateFile(_textEntered.Text);
            }
        }

        private void OnPressed(object? sender, RoutedEventArgs e)
        {
            CreateFile(_textEntered.Text);
        }

        private void OnCancelPressed(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion

        #region Process

        private void CreateFile(string name)
        {
            if (!ManagerReader.IsPathCorrect(name))
                new Views.ErrorPopUp.ErrorPopUp(_model!, new PathFormatException("Format is invalid !")).Show();
            else if (File.Exists(name))
                new Views.ErrorPopUp.ErrorPopUp(_model!, new ReplaceException("File already exists !")).Show();
            else
            {
                try
                {
                    var ft = ManagerWriter.Create(name);
                    _model!.ModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                    _model.ModelPathsBar.ReloadPath(_model.ModelNavigationBar.DirectoryPointer
                        .ChildrenFiles);
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new file";
                        new Views.ErrorPopUp.ErrorPopUp(_model!, @managerException).Show();
                    }
                }
            }
        }

        #endregion

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape)
                Close();
            if (e.Key is Key.Enter)
                CreateFile(_textEntered.Text);
        }
    }
}