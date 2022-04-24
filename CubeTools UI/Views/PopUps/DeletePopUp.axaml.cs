using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;

namespace CubeTools_UI.Views.PopUps
{
    public class DeletePopUp : Window
    {
        private MainWindowViewModel? ViewModel;

        #region Init
        
        public DeletePopUp()
        {
            InitializeComponent();
            ViewModel = null;
        }
        public DeletePopUp(MainWindowViewModel vm) : this()
        {
            ViewModel = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Events

        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Escape) Close();
            if (e.Key is Key.Enter) DeletePointer();
        }
        
        private void OnDeleteClick(object? sender, RoutedEventArgs e)
        {
            DeletePointer();
        }

        #endregion

        

        private void DeletePointer()
        {
            
        }
    }
}