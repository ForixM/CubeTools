using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using CubeTools_UI.ViewModels;
using CubeTools_UI.ViewModels.ErrorPopUp;
using CubeTools_UI.Views.PopUps;
using Library.ManagerExceptions;
using Library.Pointers;

namespace CubeTools_UI.Views
{
    public class PathsBar : UserControl
    {
        public static PathsBarViewModel ViewModel;
        private StackPanel _generator;

        public PathsBar()
        {
            InitializeComponent();
            _generator = this.FindControl<StackPanel>("Generator");
            ViewModel = new PathsBarViewModel(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ReloadPath(List<FileType> ftList)
        {
            _generator.Children.Clear();
            foreach (var ft in ftList)
            {
                var pi = new PointerItem(ft, ViewModel.ParentViewModel);
                _generator.Children.Add(pi);
            }
        }
    }
}
