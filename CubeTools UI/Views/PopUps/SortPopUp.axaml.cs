using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class SortPopUp : Window
    {
        private MainWindowViewModel _main;

        #region Init
        
        public SortPopUp()
        {
            InitializeComponent();
            _main = null;
        }
        
        public SortPopUp(MainWindowViewModel main) : this()
        {
            _main = main;
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        #region Events

        private void OnNameClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByName(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.Items =
                ManagerReader.ListToObservable(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnTypeClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByType(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.Items =
                ManagerReader.ListToObservable(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnSizeClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortBySize(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.Items =
                ManagerReader.ListToObservable(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void OnDateClick(object? sender, RoutedEventArgs e)
        {
            _main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles = ManagerReader.SortByModifiedDate(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            _main.ViewModelPathsBar.Items =
                ManagerReader.ListToObservable(_main.ViewModelNavigationBar.DirectoryPointer.ChildrenFiles);
            Close();
        }
        
        private void Cancel(object? sender, RoutedEventArgs e)
        {
            Close();
        }
        
        #endregion
        
    }
}