using System.Collections.Generic;
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
using SevenZip;

namespace CubeTools_UI.Views.Compression
{
    public class CompressPopUp : Window
    {
        private readonly MainWindowModel? _model;

        #region Visual References
        
        private TextBox _archiveNameVisual;
        private ComboBox _archiveFormatVisual;
        
        #endregion
        
        #region Variables

        private OutArchiveFormat _archiveFormat;
        private List<FileType> _datas;
        
        #endregion

        #region Init
        
        public CompressPopUp()
        {
            InitializeComponent();

            _archiveNameVisual = this.FindControl<TextBox>("NameArchive");
            _archiveFormatVisual = this.FindControl<ComboBox>("SelectedMode");
            
            _model = null;
            _datas = new List<FileType>();
        }
        public CompressPopUp(MainWindowModel vm, List<FileType> datasIn) : this()
        {
            _model = vm;
            _datas = datasIn;
            string parent = ManagerReader.GetParent(_datas[0]);
            _archiveNameVisual.Text = ManagerReader.GetPathToName(parent);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
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

            if (Directory.Exists(data) || File.Exists(data))
                data = ManagerReader.GenerateNameForModification(data) + SelectStringbyArchive(_archiveFormat);
            else
                data += SelectStringbyArchive(_archiveFormat);
            
            FileType ft = FileType.NullPointer;
            
            try
            {
                ft = ManagerWriter.CreateDir(data);
                foreach (var pointer in _datas)
                    ManagerWriter.Rename(pointer.Path, ManagerReader.GetParent(pointer) + "/" + ft.Name + "/" +pointer.Name);
            }
            catch (ManagerException e)
            {
                e.Errorstd = "Unable to compress the archive";
                _model!.View.SelectErrorPopUp(e);
            }
            
            var task = LibraryCompression.Compression.CompressDirectory(ft, data, _archiveFormat);
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