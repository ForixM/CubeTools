using System.ComponentModel;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library;
using Library.ManagerExceptions;
using Library.ManagerWriter;

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
            SystemDecorations = SystemDecorations.None;
        }
        public MoreInformation(PointerItem item, OneClient main) : this()
        {
            itemXaml = item;
            _main = main;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        private void Copy(object? sender, RoutedEventArgs e) => _main!.ActionView.Copy(sender, e);

        private void Cut(object? sender, RoutedEventArgs e) => _main!.ActionView.Cut(sender, e);

        private void Paste(object? sender, RoutedEventArgs e) => _main!.ActionView.Paste(sender, e);

        private void Rename(object? sender, RoutedEventArgs e) => _main!.ActionView.Rename(sender, e);

        private void Delete(object? sender, RoutedEventArgs e) => _main!.ActionView.Delete(sender, e);

        private void OpenWith(object? sender, RoutedEventArgs e) => _main!.AccessPath(itemXaml!.Pointer);

        private void OpenWithDefault(object? sender, RoutedEventArgs e) => _main!.AccessPath(itemXaml!.Pointer);
        
        #endregion


        private void OnEscapePressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
        }

        private void OnLostFocus(object? sender, RoutedEventArgs e) => Close();

        private void OnPropertiesOpen(object? sender, RoutedEventArgs e) => new Properties.Properties();
    }
}