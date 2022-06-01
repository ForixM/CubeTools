﻿using System;
using System.IO;
using System.Linq;
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
    public class CreatePopUp : Window
    {
        private readonly LocalModel? _model;
        private readonly TextBox _textEntered;

        #region Init
        
        public CreatePopUp()
        {
            InitializeComponent();
            _textEntered = this.FindControl<TextBox>("TextEntered");
            _model = null;
        }
        public CreatePopUp(LocalModel vm) : this()
        {
            _model = vm;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion
        
        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter) CreateFile(_textEntered.Text);
        }
        private void OnPressed(object? sender, RoutedEventArgs e) => CreateFile(_textEntered.Text);
        private void OnCancelPressed(object? sender, RoutedEventArgs e) => Close();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape)
                Close();
            if (e.Key is Key.Enter)
                CreateFile(_textEntered.Text);
        }
        
        #endregion

        #region Process

        /// <summary>
        /// Create a file with a given name
        /// </summary>
        /// <param name="name">The given name</param>
        private void CreateFile(string name)
        {
            if (_model == null) return;
            
            if (!ManagerReader.IsPathCorrect(name))
                _model.SelectErrorPopUp(new PathFormatException("Format is invalid !"));
            else if (File.Exists(_model.ModelNavigationBar.FolderPointer.Path + "/"+name))
                _model.SelectErrorPopUp(new ReplaceException("File already exists !"));
            else
            {
                try
                {
                    string[] split = name.Split(".");
                    switch (split.Length)
                    {
                        case 0:
                            _model.ModelNavigationBar.FolderPointer.AddFile("New File", "txt");
                            break;
                        case 1:
                            _model.ModelNavigationBar.FolderPointer.AddFile(name, "");
                            break;
                        default:
                        {
                            string nameNoExtension = "";
                            for (int i = 0; i < split.Length - 1; i++) nameNoExtension += split[i];
                            _model.ModelNavigationBar.FolderPointer.AddFile(nameNoExtension, split[-1]);
                            break;
                        }
                    }
                    _model.ModelPathsBar.ReloadPath(_model.ModelNavigationBar.FolderPointer.ChildrenFiles);
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new file";
                        _model.SelectErrorPopUp(@managerException);
                    }
                }
            }
        }

        #endregion
    }
}