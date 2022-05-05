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

namespace CubeTools_UI.Views.Actions
{
    public class CreateFolderPopUp : Window
    {
        private MainWindowModel _model;
        private TextBox TextEntered;

        #region Init
        
        public CreateFolderPopUp()
        {
            InitializeComponent();
            TextEntered = this.FindControl<TextBox>("TextEntered");
            _model = null;
        }
        public CreateFolderPopUp(MainWindowModel vm) : this()
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
                CreateDir(TextEntered.Text);
        }

        private void OnPressed(object? sender, RoutedEventArgs e)
        {
            CreateDir(TextEntered.Text);
        }
        
        private void OnCancelPressed(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) CreateDir(TextEntered.Text);
        }
        
        #endregion

        #region Process

        private void CreateDir(string name)
        {
            if (!ManagerReader.IsPathCorrect(name))
                TextEntered.Text = "Invalid Text !";
            else if (Directory.Exists(name))
                new Views.ErrorPopUp.ErrorPopUp(_model, new ReplaceException("File already exists !")).Show();
            else
            {
                try
                {
                    var ft = ManagerWriter.CreateDir(name);
                    _model.ModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                    _model.ModelPathsBar.ReloadPath(_model.ModelNavigationBar.DirectoryPointer
                        .ChildrenFiles);
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new folder";
                        _model.View.SelectErrorPopUp(@managerException);
                    }
                }
            }
        }

        #endregion

        
    }
}