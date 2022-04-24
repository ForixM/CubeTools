using System;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.ManagerWriter;

namespace CubeTools_UI.Views.PopUps
{
    public class CreateFolderPopUp : Window
    {
        private MainWindowViewModel ViewModel;
        private TextBox TextEntered;

        #region Init
        
        public CreateFolderPopUp()
        {
            InitializeComponent();
            TextEntered = this.FindControl<TextBox>("TextEntered");
            ViewModel = null;
        }
        public CreateFolderPopUp(MainWindowViewModel vm) : this()
        {
            ViewModel = vm;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Events

        private void OnEnterPressed(object? sender, KeyEventArgs e)
        {
            if (e.Key is Key.Enter)
                CreateFile(TextEntered.Text);
        }

        private void OnPressed(object? sender, RoutedEventArgs e)
        {
            CreateFile(TextEntered.Text);
        }
        
        private void OnCancelPressed(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion

        #region Process

        private void CreateFile(string name)
        {
            if (!ManagerReader.IsPathCorrect(name))
                new Views.ErrorPopUp.ErrorPopUp(ViewModel, new PathFormatException("Format is invalid !")).Show();
            else if (Directory.Exists(name))
                new Views.ErrorPopUp.ErrorPopUp(ViewModel, new ReplaceException("File already exists !")).Show();
            else
            {
                try
                {
                    var ft = ManagerWriter.CreateDir(name);
                    ViewModel.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles.Add(ft);
                    ViewModel.ViewModelPathsBar.ReloadPath(ViewModel.ViewModelNavigationBar.DirectoryPointer
                        .ChildrenFiles);
                    Close();
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to create a new folder";
                        new Views.ErrorPopUp.ErrorPopUp(ViewModel, @managerException).Show();
                    }
                }
            }
        }

        #endregion
        
    }
}