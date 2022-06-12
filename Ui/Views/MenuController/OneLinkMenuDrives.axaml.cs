using System;
using System.IO;
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
using Ui.Views.Information;

namespace Ui.Views.LinkBar
{
    public class OneLinkMenuDrives : UserControl
    {
        public ClientUI Main;
        public LocalPointer LocalPointer;
        public TextBlock Description;
        public Image Image;
        private ProgressBar runProgress;
        private TextBlock SpaceInfo;
        
        public OneLinkMenuDrives()
        {
            LocalPointer = LocalPointer.NullLocalPointer;
            
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
            SpaceInfo = this.FindControl<TextBlock>("SpaceInfo");
        }

        public OneLinkMenuDrives(ClientUI main, string link, string name, IImage image) : this()
        {
            Main = main;
            try
            {
                if (Directory.Exists(link)) LocalPointer = new DirectoryLocalPointer(link);
                else LocalPointer = new FileLocalPointer(link);
            }
            catch (PathNotFoundException)
            {
                LocalPointer = LocalPointer.NullLocalPointer;
            }

            Description.Text = name;
            Image.Source = image;
            runProgress = this.FindControl<ProgressBar>("RunProgress");
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (drive.Name == link)
                {
                    runProgress.Value = 100d - (double)((double)drive.AvailableFreeSpace / (double)drive.TotalSize) * 100.0d;
                    SpaceInfo.Text = $"{ManagerReader.ByteToPowByte(drive.AvailableFreeSpace)} free of {ManagerReader.ByteToPowByte(drive.TotalSize)}";
                }
            }
            // runProgress.Value = 50;
            // DriveInfo info = DriveInfo.GetDrives()[0];
            // info.TotalSize - info.AvailableFreeSpace;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenLink(object? sender, RoutedEventArgs e) => Main.AccessPath(LocalPointer);
        
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            /*
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                new MoreInformationLink(this, Main).Show();
                */
        }
    }
}
