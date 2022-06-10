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
    public class OneLink : UserControl
    {
        public ClientUI Main;
        public LocalPointer LocalPointer;
        public TextBlock Description;
        public Image Image;
        
        public OneLink()
        {
            // Main = ClientUI.LastReference;
            LocalPointer = LocalPointer.NullLocalPointer;
            
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
        }

        public OneLink(ClientUI main, string link, string name, IImage image) : this()
        {
            Main = main;
            ToolTip.SetTip(this, name);
            try
            {
                if (Directory.Exists(link)) LocalPointer = new DirectoryLocalPointer(link);
                else LocalPointer = new FileLocalPointer(link);
            }
            catch (PathNotFoundException)
            {
                ConfigLoader.ConfigLoader.Settings.Links.Remove(ManagerReader.GetPathToName(link));
                ConfigLoader.ConfigLoader.SaveConfiguration();
                LocalPointer = LocalPointer.NullLocalPointer;
            }

            Description.Text = name;
            Image.Source = image;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenLink(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (Main.Main is MainWindowRemote)
                    ((MainWindowRemote) Main.Main).LocalView.AccessPath(LocalPointer);
                else
                    ((MainWindow) Main.Main).LocalView.AccessPath(LocalPointer);
            }
            catch (PathNotFoundException exception)
            {
                ConfigLoader.ConfigLoader.Settings.Links.Remove(ManagerReader.GetPathToName(LocalPointer.Path));
                ConfigLoader.ConfigLoader.SaveConfiguration();
            }
        }
        
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
                new MoreInformationLink(this, Main).Show();
        }
    }
}
