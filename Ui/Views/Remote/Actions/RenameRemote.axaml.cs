using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using LibraryClient;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.Remote.Actions
{
    public class RenameRemote : Window
    {
        private readonly TextBox _renameBox;
        private readonly RemoteItem? _modifiedPointer;
        private readonly List<RemoteItem> _itemsReference;
        private readonly MainWindowRemote? _main;

        #region Init
        public RenameRemote()
        {
            InitializeComponent();
            _renameBox = this.FindControl<TextBox>("Rename");
            _modifiedPointer = null;
            _itemsReference = new List<RemoteItem>();
            _main = null;
        }
        public RenameRemote(RemoteItem pointer, List<RemoteItem> items, MainWindowRemote main) : this()
        {
            _modifiedPointer = pointer;
            _itemsReference = items;
            _renameBox.Text = _modifiedPointer.Name;
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
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
            if (_modifiedPointer is null || _main is null) Close();
            else
            {
                if (_main.Client.GetItem(_renameBox.Text) is null)
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
                        _main.Client.Rename(_modifiedPointer, _renameBox.Text);
                        _itemsReference[index] = _modifiedPointer;
                    }
                    catch (ManagerException exception)
                    {
                        new ErrorBase(exception).ShowDialog<object>(_main!);
                    }

                    _main.Refresh();
                    Close();
                }

                if (_modifiedPointer.Name == _renameBox.Text)
                {
                    _main.Refresh();
                    Close();
                }
            }
        }
    }
}