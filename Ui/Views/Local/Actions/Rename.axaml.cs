using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Pointer = Library.Pointer;

namespace Ui.Views.Local.Actions
{
    public class Rename : Window
    {
        private readonly TextBox _renameBox;
        private readonly Pointer _modifiedPointer;
        private readonly List<Pointer> _itemsReference;
        private readonly Local? _main;

        #region Init
        public Rename()
        {
            InitializeComponent();
            _renameBox = this.FindControl<TextBox>("Rename");
            _modifiedPointer = Pointer.NullPointer;
            _itemsReference = new List<Pointer>();
            _main = null;
        }
        public Rename(Pointer ft, List<Pointer> items, Local main) : this()
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
        
        /// <summary>
        /// Perform the action
        /// </summary>
        private void RenamePointer()
        {
            if (File.Exists(_renameBox.Text) || Directory.Exists(_renameBox.Text))
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
                    int index = _itemsReference.IndexOf(_modifiedPointer);
                    ManagerWriter.Rename(_modifiedPointer, _renameBox.Text, false);
                    _itemsReference[index] = _modifiedPointer;
                }
                catch (ManagerException exception)
                {
                    _main.SelectErrorPopUp(exception);
                }
                _main.Refresh();
                Close();
            }

            if (_modifiedPointer.Path == "" || _modifiedPointer.Name == _renameBox.Text)
            {
                _main.Refresh();
                Close();
            }
        }
    }
}