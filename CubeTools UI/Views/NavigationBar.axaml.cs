using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;

namespace CubeTools_UI.Views
{
    public class NavigationBar : UserControl
    {
        public static NavigationBarViewModel ViewModel;
        public TextBox CurrentPathXaml;
        
        public NavigationBar()
        {
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("CurrentPath");
            ViewModel = new NavigationBarViewModel(this);
            DataContext = ViewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (((TextBox) sender!)?.DataContext is NavigationBarViewModel navigationBarViewModel)
            {
                navigationBarViewModel.ParentViewModelXaml.AccessPath(((TextBox) sender).Text);
            }
        }
    }
}
