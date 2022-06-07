using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Library.FilePointer;
using Library.ManagerExceptions;
using Pointer = Library.Pointer;

namespace Ui.Views.MainWindow.DynamicLinkBar
{
    public class OneLink : UserControl
    {
        public Local.Local Main;
        public Pointer pointer;
        public TextBlock Description;
        public Image Image;
        
        public OneLink()
        {
            Main = Local.Local.LastReference;
            pointer = Pointer.NullPointer;
            
            InitializeComponent();
            Description = this.FindControl<TextBlock>("Description");
            Image = this.FindControl<Image>("Image");
        }

        public OneLink(string link, string name, IImage image) : this()
        {
            try
            {
                if (Directory.Exists(link)) pointer = new DirectoryPointerLoaded(link);
                else pointer = new FilePointer(link);
            }
            catch (PathNotFoundException)
            {
                pointer = Pointer.NullPointer;
            }

            Description.Text = name;
            Image.Source = image;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OpenLink(object? sender, RoutedEventArgs e) => Main.AccessPath(pointer.Path);
        
        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                
            }
        }
    }
}
