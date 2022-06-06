using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerWriter;
using Pointer = Library.Pointer;

namespace Ui.Views.Information
{
    public class MoreInformation : Window
    {
        #region Children Components

        private Image _imageExtension;
        private TextBlock _fileName;
        private TextBlock _type;
        private TextBlock _description;
        private TextBlock _path;
        private TextBlock _size;
        private TextBlock _created;
        private TextBlock _modified;
        private TextBlock _accessed;
        private CheckBox _readOnly;
        private CheckBox _hidden;

        #endregion

        private bool _userActivation;

        private readonly Pointer _pointer;
        
        #region Init
        public MoreInformation()
        {
            InitializeComponent();
            _pointer = Pointer.NullPointer;
            
            _imageExtension = this.FindControl<Image>("ImageExtension");
            _fileName = this.FindControl<TextBlock>("FileName");
            _type = this.FindControl<TextBlock>("Type");
            _description = this.FindControl<TextBlock>("Description");
            _path = this.FindControl<TextBlock>("Path");
            _size = this.FindControl<TextBlock>("Size");
            _created = this.FindControl<TextBlock>("Created");
            _modified = this.FindControl<TextBlock>("Modified");
            _accessed = this.FindControl<TextBlock>("Accessed");
            _readOnly = this.FindControl<CheckBox>("Read-Only");
            _hidden = this.FindControl<CheckBox>("Hidden");

            _userActivation = false;
        }
        public MoreInformation(Pointer pointer) : this()
        {
            _pointer = pointer;
            /*
            _imageExtension.Source = ResourcesLoader.ResourcesConverter.TypeToIcon(pointer.Type, pointer.IsDir);
            _fileName.Text = pointer.Name;
            _type.Text = pointer.IsDir ? "folder" : pointer.Type;
            _description.Text = pointer.Name;
            _path.Text = pointer.Path;
            _size.Text = pointer.SizeXaml;
            _created.Text = pointer.Date;
            _modified.Text = pointer.LastDate;
            _accessed.Text = pointer.AccessDate;
            _readOnly.IsChecked = pointer.ReadOnly;
            _hidden.IsChecked = pointer.Hidden;*/
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void Copy(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void Cut(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void Paste(object? sender, RoutedEventArgs e)
        {
            
        }

        private void Rename(object? sender, RoutedEventArgs e)
        {
            
        }

        private void Compression(object? sender, RoutedEventArgs e)
        {
            
        }

        private void Delete(object? sender, RoutedEventArgs e)
        {
            
        }
        
        private void OpenWith(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        
        private void OpenWithDefault(object? sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
        
        #endregion


        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }
    }
}