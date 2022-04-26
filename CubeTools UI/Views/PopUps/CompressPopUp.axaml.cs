using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.Pointers;
using LibraryCompression;
using SevenZip;

namespace CubeTools_UI.Views.PopUps
{
    public class CompressPopUp : Window
    {
        private MainWindowViewModel ViewModel;

        #region Visual References
        
        private TextBox _archiveNameVisual;
        private ComboBox _archiveFormatVisual;
        
        #endregion
        
        #region Variables

        private OutArchiveFormat _archiveFormat;
        private List<FileType> _data;
        
        #endregion

        #region Init
        
        public CompressPopUp()
        {
            InitializeComponent();

            _archiveNameVisual = this.FindControl<TextBox>("NameArchive");
            _archiveFormatVisual = this.FindControl<ComboBox>("SelectedMode");
            
            ViewModel = null;
            _data = new List<FileType>();
        }
        public CompressPopUp(MainWindowViewModel vm, List<FileType> dataIn) : this()
        {
            ViewModel = vm;
            _data = dataIn;
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
                int size = _data.Count;
                for (int i = 0; i < size; i++)
                {
                    if (!File.Exists(_data[i].Path))
                    {
                        _data.RemoveAt(i);
                        size--;
                    }
                }

                if (_data.Count > 0)
                {
                    Task task = Compression.CompressFiles(_data.ToArray(), _archiveFormat);
                    task.GetAwaiter().OnCompleted(Close);
                }
            }
        }

        private void OnCompressionPressed(object? sender, RoutedEventArgs e)
        {
            OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});
        }
        
        private void OnCancelPressed(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});
        }
        
        private void OnCompressModeChanged(object? sender, SelectionChangedEventArgs e)
        {
            var mode = ((TextBlock) e.AddedItems[0]!).Text;
            _archiveFormat = SelectArchiveByString(mode);
        }
        
        #endregion

        #region Process

        private void CompressFiles()
        {
            if (Directory.Exists(_archiveNameVisual.Text) || File.Exists(_archiveNameVisual.Text))
            {
                
            }
            else
            {
                var task = Compression.CompressFiles(_data.ToArray(), _archiveNameVisual.Text, _archiveFormat);
                task.GetAwaiter().OnCompleted(() =>
                {
                    ViewModel.ReloadPath();
                    Close();
                });
            }
        }

        private OutArchiveFormat SelectArchiveByString(string archive)
        {
            return archive switch
            {
                "Zip" => OutArchiveFormat.Zip,
                "7zip" => OutArchiveFormat.SevenZip,
                _ => OutArchiveFormat.SevenZip
            };
        }

        #endregion
    }
}