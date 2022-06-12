using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library;
using Library.DirectoryPointer;
using Library.FilePointer;
using Library.ManagerExceptions;
using Library.ManagerReader;

namespace Ui.Views.MenuController
{
    public class OneLinkMenuDrives : UserControl
    {
        private ClientUI _main;
        private LocalPointer _localPointer;
        private TextBlock _description;
        private Image _image;
        private ProgressBar _progressBar;
        private ProgressBar runProgress;
        private TextBlock _spaceInfo;
        
        public OneLinkMenuDrives()
        {
            _localPointer = LocalPointer.NullLocalPointer;
            
            InitializeComponent();
            _description = this.FindControl<TextBlock>("Description");
            _image = this.FindControl<Image>("Image");
            _progressBar = this.FindControl<ProgressBar>("ProgressBar");
            _spaceInfo = this.FindControl<TextBlock>("SpaceInfo");
        }

        public OneLinkMenuDrives(ClientUI main, string link, string name, IImage image) : this()
        {
            _main = main;
            try
            {
                if (Directory.Exists(link)) _localPointer = new DirectoryLocalPointer(link);
                else _localPointer = new FileLocalPointer(link);
            }
            catch (PathNotFoundException)
            {
                _localPointer = LocalPointer.NullLocalPointer;
            }
            _description.Text = $"{name} ({link})";
            _image.Source = image;
            runProgress = this.FindControl<ProgressBar>("RunProgress");
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.Name == link)
                {
                    runProgress.Value = 100d - (drive.AvailableFreeSpace / (double)drive.TotalSize) * 100.0d;
                    switch (runProgress.Value)
                    {
                        case >= 90:
                            runProgress.Foreground = Brush.Parse("#ff5900");
                            break;
                        case >= 75:
                            runProgress.Foreground = Brush.Parse("#ff9e00");
                            break;
                    }
                    _spaceInfo.Text = $"{ManagerReader.ByteToPowByte(drive.AvailableFreeSpace)} free of {ManagerReader.ByteToPowByte(drive.TotalSize)}";
                }
            }
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenLink(object? sender, RoutedEventArgs e) => _main.AccessPath(_localPointer);
        
    }
}
