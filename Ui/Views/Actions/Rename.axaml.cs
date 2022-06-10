using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class Rename : Window
    {
        private readonly TextBox _renameBox;
        private readonly Pointer _modifiedLocalPointer;
        private readonly List<Pointer> _itemsReference;
        private readonly ClientUI? _main;

        #region Init
        public Rename()
        {
            InitializeComponent();
            _renameBox = this.FindControl<TextBox>("Rename");
            _modifiedLocalPointer = LocalPointer.NullLocalPointer;
            _itemsReference = new List<Pointer>();
            _main = null;
        }
        public Rename(Pointer ft, List<Pointer> items, ClientUI main) : this()
        {
            _modifiedLocalPointer = ft;
            _itemsReference = items;
            _renameBox.Text = _modifiedLocalPointer.Name;
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
        
        private void OnClickRename(object? sender, RoutedEventArgs e) =>  RenamePointer();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter) RenamePointer();
            else if (e.Key is Key.Escape) Close();
        }
        
        #endregion
        
        /// <summary>
        /// Perform the action
        /// </summary>
        private async void RenamePointer()
        {
            if (_modifiedLocalPointer.Name == _renameBox.Text)
                Close(null);
            else if (File.Exists(_renameBox.Text) || Directory.Exists(_renameBox.Text))
            {
                //_modifiedPointer.RenameAsync()();
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
                    _main!.Client.Rename(_modifiedLocalPointer, _renameBox.Text);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException managerException) await new ErrorBase(managerException).ShowDialog<object>(_main?.Main);
                }
                _main?.Refresh();
                Close(null);
            }
        }
    }
}