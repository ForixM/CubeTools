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

namespace CubeTools_UI.Views.Ftp
{
    public class LocalFTP : UserControl
    {
        public StackPanel Generator;

        public LocalFTP()
        {
            InitializeComponent();
            Generator = this.FindControl<StackPanel>("LocalGenerator");
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
                /*
                var pi = new LocalPointer(ft, ViewModel.ParentViewModel);
                if (ViewModel.ParentViewModel.ViewModelActionBar.SelectedXaml.Contains(pi))
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                else
                    pi.button.Background = new SolidColorBrush(new Color(255, 255, 255, 255));
                Generator.Children.Add(pi);
                */
            }
        }
    }
}
