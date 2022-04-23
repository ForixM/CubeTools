using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class LoadingPopUp : Window
    {
        public ProgressBar progressBar;
        private bool _destroy;

        public LoadingPopUp()
        {
            InitializeComponent();
            progressBar = this.FindControl<ProgressBar>("ProgressBar");
        }
        public LoadingPopUp(bool destroy=false) : this()
        {
            _destroy = destroy;
        }

        public void ReloadProgress(int nb, int max)
        {
            if (_destroy)
                progressBar.Value = nb/max;
            else
                progressBar.Value = (max - nb) / max;
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}