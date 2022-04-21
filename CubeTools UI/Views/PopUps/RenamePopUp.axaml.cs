using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class RenamePopUp : Window
    {
        private readonly TextBox _renameBox;
        private readonly FileType _modifiedPointer;
        private ObservableCollection<FileType> _itemsReference;

        public RenamePopUp()
        {
            InitializeComponent();
            _renameBox = this.FindControl<TextBox>("Rename");
            _modifiedPointer = FileType.NullPointer;
            _itemsReference = new ObservableCollection<FileType>();
        }
        public RenamePopUp(FileType ft, ObservableCollection<FileType> items) : this()
        {
            _modifiedPointer = ft;
            _itemsReference = items;
            _renameBox.Text = _modifiedPointer.Name;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnRename(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
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
                        Close();
                    }
                    catch (ManagerException exception)
                    {
                        // TODO Display error !
                    }
                }
            }

            if (_modifiedPointer.Path == "")
                Close();
        }
    }
}