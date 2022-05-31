using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;
using Library;
using SevenZip;
using Ui.Models;

namespace Ui.Views.Compression
{
    public class CompressPopUp : Window
    {
        private readonly LocalModel? _model;
        
        private readonly TextBox _archiveNameVisual;
        private readonly ComboBox _archiveFormatVisual;
        
        private OutArchiveFormat _archiveFormat;
        private List<FilePointer> _datas;

        #region Init
        
        public CompressPopUp()
        {
            InitializeComponent();

            _archiveNameVisual = this.FindControl<TextBox>("NameArchive");
            _archiveFormatVisual = this.FindControl<ComboBox>("SelectedMode");
            _archiveFormat = OutArchiveFormat.Zip;
            
            _model = null;
            _datas = new List<FilePointer>();
        }
        public CompressPopUp(LocalModel vm, List<FilePointer> datasIn) : this()
        {
            _model = vm;
            _datas = datasIn;
            string parent = ManagerReader.GetParent(_datas[0]);
            _archiveNameVisual.Text = ManagerReader.GetPathToName(parent);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (_datas.Count == 0) return;
            if (e.Key is Key.Enter) Compress();
        }

        private void OnCompressionPressed(object? sender, RoutedEventArgs e) => OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});

        private void OnCancelPressed(object? sender, RoutedEventArgs e) => Close();

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) Compress();
        }
        
        private void OnCompressModeChanged(object? sender, SelectionChangedEventArgs e)
        {
            var mode = ((TextBlock) e.AddedItems[0]!).Text;
            _archiveFormat = SelectArchiveByString(mode);
        }
        
        #endregion

        #region Process

        private void Compress()
        {
            string data = _archiveNameVisual.Text;
            data += SelectStringbyArchive(_archiveFormat);
            var task = LibraryCompression.Compression.CompressItems(_datas, data, _archiveFormat);
            task.Start();
            task.GetAwaiter().OnCompleted(() =>
            {
                _model!.ReloadPath();
                Close();
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archive"></param>
        /// <returns></returns>
        private OutArchiveFormat SelectArchiveByString(string archive)
        {
            return archive switch
            {
                ".zip" => OutArchiveFormat.Zip,
                ".7z" => OutArchiveFormat.SevenZip,
                ".tar" => OutArchiveFormat.Tar,
                _ => OutArchiveFormat.Zip
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private string SelectStringbyArchive(OutArchiveFormat format)
        {
            return format switch
            {
                OutArchiveFormat.Zip => ".zip",
                OutArchiveFormat.SevenZip => ".7z",
                OutArchiveFormat.Tar => ".tar",
                _ => ".zip"
            };
        }

        #endregion
    }
}