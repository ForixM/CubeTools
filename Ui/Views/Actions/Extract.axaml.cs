using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library;
using SevenZip;
using Pointer = Library.Pointer;

namespace Ui.Views.Actions
{
    public class Extract : Window
    {
        private ClientUI? _main;

        private StackPanel _archiveInfo;
        private List<Pointer> _data;

        #region Init
        
        public Extract()
        {
            InitializeComponent();

            _archiveInfo = this.FindControl<StackPanel>("ArchiveInformation");
            _main = null;
            _data = new List<Pointer>();
        }
        public Extract(ClientUI main, List<Pointer> dataIn) : this()
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
                Thread compressionThread = new Thread(() =>
                {
                    foreach (LocalPointer pointer in _data.Where(ft => ft is LocalPointer))
                        LibraryCompression.Compression.Extract(pointer);
                    Dispatcher.UIThread.Post(() =>
                    {
                        _main.Refresh();
                        Close();
                    }, DispatcherPriority.Render);
                });
                compressionThread.Start();
            }
        }

        private void OnCancelPressed(object? sender, RoutedEventArgs routedEventArgs) => Close();
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) OnEnterPressed(sender, new KeyEventArgs() { Key = Key.Enter});
        }

        private void Extraction(object? sender, RoutedEventArgs routedEventArgs)
        {
            Thread compressionThread = new Thread(() =>
            {
                foreach (LocalPointer pointer in _data.Where(ft => ft is LocalPointer))
                    LibraryCompression.Compression.Extract(pointer);
                Dispatcher.UIThread.Post(() =>
                {
                    _main.Refresh();
                    Close();
                }, DispatcherPriority.Render);
            });
            compressionThread.Start();
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

        private void OnKeyReleased(object? sender, KeyEventArgs e)
        {
            if (_main.Main is MainWindow window)
            {
                MainWindow.KeysPressed.Remove(e.Key);
            }
            else if (_main.Main is MainWindowRemote windowRemote)
            {
                windowRemote.KeysPressed.Remove(e.Key);
            };
        }
    }
}