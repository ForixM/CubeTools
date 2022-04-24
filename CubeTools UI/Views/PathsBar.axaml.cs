using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public StackPanel Generator;

        public PathsBar()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("Generator");
            ViewModel = new PathsBarViewModel(this);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void ReloadPath(List<FileType> ftList)
        {
            Generator.Children.Clear();
            foreach (var ft in ftList)
            {
                var pi = new PointerItem(ft, ViewModel.ParentViewModel);
                if (ViewModel.ParentViewModel.ViewModelActionBar.SelectedXaml.Contains(pi))
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
            }
        }
    }
}
