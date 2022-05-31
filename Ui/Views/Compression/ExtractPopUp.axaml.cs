using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library;
using SevenZip;
using Ui.Models;

namespace Ui.Views.Compression
{
    public class ExtractPopUp : Window
    {
        private LocalModel? _model;

        private StackPanel _archiveInfo;
        private List<FilePointer> _data;

        #region Init
        
        public ExtractPopUp()
        {
            InitializeComponent();

            _archiveInfo = this.FindControl<StackPanel>("ArchiveInformation");
            _model = null;
            _data = new List<FilePointer>();
        }
        public ExtractPopUp(LocalModel vm, List<FilePointer> dataIn) : this()
        {
            _model = vm;
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
                Task task = Task.Run(() =>
                {
                    foreach (var ft in _data)
                        LibraryCompression.Compression.Extract(ft);
                });
                task.GetAwaiter().OnCompleted(Close);
            }
        }

        private void OnCancelPressed(object? sender, PointerPressedEventArgs e) => Close();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});
        }

        private void Extract(object? sender, PointerPressedEventArgs e)
        {
            Task task = Task.Run(() =>
            {
                foreach (var ft in _data)
                    LibraryCompression.Compression.Extract(ft);
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