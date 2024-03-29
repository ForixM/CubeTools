﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using Library.ManagerReader;
using SevenZip;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class Compress : Window
    {
        private readonly ClientUI? _main;
        
        private readonly TextBox _archiveNameVisual;
        private readonly ComboBox _archiveFormatVisual;
        
        private OutArchiveFormat _archiveFormat;
        private List<Pointer> _datas;

        #region Init
        
        public Compress()
        {
            InitializeComponent();

            _archiveNameVisual = this.FindControl<TextBox>("NameArchive");
            _archiveFormatVisual = this.FindControl<ComboBox>("SelectedMode");
            _archiveFormat = OutArchiveFormat.Zip;
            
            _main = null;
            _datas = new List<Pointer>();
        }
        public Compress(ClientUI main, List<Pointer> datasIn) : this()
        {
            _main = main;
            _datas = datasIn;
            // TODO
            //string parent = ManagerReader.GetParent(_datas[0]);
            //_archiveNameVisual.Text = ManagerReader.GetPathToName(parent);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (_datas.Count == 0) return;
            if (e.Key is Key.Enter) Compression();
        }

        private void OnCompressionPressed(object? sender, RoutedEventArgs e) => OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});

        private void OnCancelPressed(object? sender, RoutedEventArgs e) => Close();

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) Compression();
        }
        
        private void OnCompressModeChanged(object? sender, SelectionChangedEventArgs e)
        {
            var mode = ((TextBlock) e.AddedItems[0]!).Text;
            _archiveFormat = SelectArchiveByString(mode);
        }
        
        #endregion

        #region Process

        private void Compression()
        {
            string data = _archiveNameVisual.Text;
            data += SelectStringbyArchive(_archiveFormat);
            Dispatcher.UIThread.Post(() =>
            {
                Task task = LibraryCompression.Compression.CompressItems(_datas,
                    _main.Client.CurrentFolder.Path + "/" + data, _archiveFormat);
                task.GetAwaiter().OnCompleted(() =>
                {
                    _main!.Refresh();
                    Close();
                });
                task.Start();
            }, DispatcherPriority.MaxValue);
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

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main.Main is MainWindow window)
            {
                MainWindow.KeysPressed.Remove(e.Key);
            }
            else if (_main.Main is MainWindowRemote windowRemote)
            {
                windowRemote.KeysPressed.Remove(e.Key);
            }
        }
    }
}