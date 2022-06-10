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
using Ui.Views.Information;

namespace Ui.Views.LinkBar
{
    public class OneLinkMenu : UserControl
    {
        public ClientUI Main;
        public LocalPointer LocalPointer;
        public TextBlock Description;
        public Image Image;
        
        public OneLinkMenu()
        {
            Main = ClientUI.LastReference;
            LocalPointer = LocalPointer.NullLocalPointer;
            
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
        }

        public OneLinkMenu(string link, string name, IImage image) : this()
        {
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
