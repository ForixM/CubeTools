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
using Library.ManagerWriter;
using Ui.Views.Error;

namespace Ui.Views.Local.Actions
{
    public class Rename : Window
    {
        private readonly TextBox _renameBox;
        private readonly LocalPointer _modifiedLocalPointer;
        private readonly List<LocalPointer> _itemsReference;
        private readonly Local? _main;

        #region Init
        public Rename()
        {
            InitializeComponent();
            _renameBox = this.FindControl<TextBox>("Rename");
            _modifiedLocalPointer = LocalPointer.NullLocalPointer;
            _itemsReference = new List<LocalPointer>();
            _main = null;
        }
        public Rename(LocalPointer ft, List<LocalPointer> items, Local main) : this()
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
                    _modifiedLocalPointer.Rename(_renameBox.Text, false);
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