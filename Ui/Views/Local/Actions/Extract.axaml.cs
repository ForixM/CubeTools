﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using SevenZip;

namespace Ui.Views.Local.Actions
{
    public class Extract : Window
    {
        private Local? _main;

        private StackPanel _archiveInfo;
        private List<LocalPointer> _data;

        #region Init
        
        public Extract()
        {
            InitializeComponent();

            _archiveInfo = this.FindControl<StackPanel>("ArchiveInformation");
            _main = null;
            _data = new List<LocalPointer>();
        }
        public Extract(Local main, List<LocalPointer> dataIn) : this()
        {
            _main = main;
            _data = dataIn;
            _archiveInfo.Children.Clear();
            foreach (var ft in _data) _archiveInfo.Children.Add(new TextBlock { Text = ft.Name});
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
            {
                Dispatcher.UIThread.Post(() =>
                {
                    Task task = Task.Run(() =>
                    {
                        foreach (var ft in _data)
                            LibraryCompression.Compression.Extract(ft);
                    });
                    task.GetAwaiter().OnCompleted(Close);
                }, DispatcherPriority.MaxValue);
            }
        }

        private void OnCancelPressed(object? sender, PointerPressedEventArgs e) => Close();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});
        }

        private void Extraction(object? sender, PointerPressedEventArgs e)
        {
            Task task = Task.Run(() =>
            {
                foreach (var pointer in _data)
                    LibraryCompression.Compression.Extract(pointer);
            });
            task.GetAwaiter().OnCompleted(Close);
        }
        
        #endregion

        private OutArchiveFormat SelectArchiveByString(string archive)
        {
            return archive switch
            {
                ".zip" => OutArchiveFormat.Zip,
                ".7z" => OutArchiveFormat.SevenZip,
                _ => OutArchiveFormat.SevenZip
            };
        }
    }
}