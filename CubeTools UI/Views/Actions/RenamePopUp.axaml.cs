﻿using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views.Actions
{
    public class RenamePopUp : Window
    {
        private readonly TextBox _renameBox;
        private readonly FileType _modifiedPointer;
        private readonly List<FileType> _itemsReference;
        private readonly LocalModel _main;

        #region Init
        public RenamePopUp()
        {
            InitializeComponent();
            _renameBox = this.FindControl<TextBox>("Rename");
            _modifiedPointer = FileType.NullPointer;
            _itemsReference = new List<FileType>();
            _main = null;
        }
        public RenamePopUp(FileType ft, List<FileType> items, LocalModel main) : this()
        {
            _modifiedPointer = ft;
            _itemsReference = items;
            _renameBox.Text = _modifiedPointer.Name;
            _main = main;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion
        
        #region Events
        
        private void OnRename(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter) RenamePointer();
        }
        
        private void OnClickRename(object? sender, RoutedEventArgs e)
        {
            RenamePointer();
        }
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter) RenamePointer();
            else if (e.Key is Key.Escape) Close();
                
        }
        
        #endregion
        
        private void RenamePointer()
        {
            if (File.Exists(_renameBox.Text) || Directory.Exists(_renameBox.Text))
            {
                // File Already exists, try to overwrite ?
            }
            else if (!ManagerReader.IsPathCorrect(_renameBox.Text))
            {
                _renameBox.Text = "";
                _renameBox.Watermark = "Incorrect characters !";
            }
            else
            {
                try
                {
                    int index = _itemsReference.IndexOf(_modifiedPointer);
                    ManagerWriter.Rename(_modifiedPointer, _renameBox.Text, false);
                    _itemsReference[index] = _modifiedPointer;
                }
                catch (ManagerException exception)
                {
                    _main.SelectErrorPopUp(exception);
                }
                _main.ReloadPath();
                Close();
            }

            if (_modifiedPointer.Path == "" || _modifiedPointer.Name == _renameBox.Text)
            {
                _main.ReloadPath();
                Close();
            }
        }
    }
}