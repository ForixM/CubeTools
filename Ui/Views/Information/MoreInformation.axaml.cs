using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Ui.Views.Error;

namespace Ui.Views.Information
{
    public class MoreInformation : Window
    {

        private readonly OneClient? _main;
        private readonly PointerItem? itemXaml;
        
        #region Init
        public MoreInformation()
        {
            InitializeComponent();
            SystemDecorations = SystemDecorations.BorderOnly;
        }
        public MoreInformation(PointerItem item, OneClient main) : this()
        {
            Position = new PixelPoint(-(int) new MouseDevice().GetPosition(item).X, 50-(int) new MouseDevice().GetPosition(item).Y);
            itemXaml = item;
            _main = main;
            if (_main.Client.Type is not ClientType.LOCAL) this.FindControl<Button>("Compress").IsVisible = false;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void Copy(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Copy(sender, e);
            Close();
        }

        private void Cut(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Cut(sender, e);
            Close();
        }

        private void Rename(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Rename(sender, e);
            Close();
        }

        private void Delete(object? sender, RoutedEventArgs e)
        {
            _main!.ActionView.Delete(sender, e);
            Close();
        }

        private void OpenWith(object? sender, RoutedEventArgs e)
        {
            _main!.AccessPath(itemXaml!.Pointer);
            Close();
        }

        private void OpenWithDefault(object? sender, RoutedEventArgs e)
        {
            _main!.AccessPath(itemXaml!.Pointer);
            Close();
        }

        private void CopyAsPath(object? sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current!.Clipboard!.SetTextAsync(itemXaml!.Pointer.Path);
            }
            catch (Exception) {}
        }
        
        private void OnPropertiesOpen(object? sender, RoutedEventArgs e)
        {
            try
            {
                new Properties.Properties(itemXaml!.Pointer, _main!.Client).Show();
            }
            catch (Exception) {}
        }
        
        private void CompressPressed(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (_main!.Client.Type is ClientType.LOCAL)
                {
                    //_main.Client.Compress();
                }
            }
            catch (ManagerException exception)
            {
                new ErrorBase(exception).Show();
            }
            
        }
        
        #endregion


        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }

        private void OnPointerLeave(object? sender, PointerEventArgs e) => Close();
    }
}