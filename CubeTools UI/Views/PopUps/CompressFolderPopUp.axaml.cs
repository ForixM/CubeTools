using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.Pointers;
using LibraryCompression;
using SevenZip;

namespace CubeTools_UI.Views.PopUps
{
    public class CompressFolderPopUp : Window
    {
        private readonly MainWindowModel? _model;

        #region Visual References
        
        private TextBox _archiveNameVisual;
        private ComboBox _archiveFormatVisual;
        
        #endregion
        
        #region Variables

        private OutArchiveFormat _archiveFormat;
        private FileType _folder;
        
        #endregion

        #region Init
        
        public CompressFolderPopUp()
        {
            InitializeComponent();

            _archiveNameVisual = this.FindControl<TextBox>("NameArchive");
            _archiveFormatVisual = this.FindControl<ComboBox>("SelectedMode");
            
            _model = null;
            _folder = FileType.NullPointer;
        }
        public CompressFolderPopUp(MainWindowModel vm, FileType folderIn) : this()
        {
            _model = vm;
            _folder = folderIn;
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
                if (Directory.Exists(_folder.Path))
                    CompressFolder();
                else
                {
                    _model!.ReloadPath();
                    Close();
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

        private void CompressFolder()
        {
            if (Directory.Exists(_archiveNameVisual.Text) || File.Exists(_archiveNameVisual.Text))
            {
                
            }
            else
            {
                var task = Compression.CompressDirectory(_folder, _archiveNameVisual.Text, _archiveFormat);
                task.GetAwaiter().OnCompleted(() =>
                {
                    _model!.ReloadPath();
                    Close();
                });
            }
        }

        private OutArchiveFormat SelectArchiveByString(string archive)
        {
            return archive switch
            {
                ".zip" => OutArchiveFormat.Zip,
                ".7z" => OutArchiveFormat.SevenZip,
                ".tar" => OutArchiveFormat.Tar,
                _ => OutArchiveFormat.SevenZip
            };
        }

        #endregion
    }
}