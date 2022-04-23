using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.ViewModels;
using CubeTools_UI.ViewModels.PopUps;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class LoadingPopUp : Window
    {
        public bool ProcessFinished;
        private ProgressBar _progress;
        private TextBlock _operationType;
        private LoadingPopUpViewModel _viewModel;

        #region Init
        
        public LoadingPopUp()
        {
            InitializeComponent();
            _progress = this.FindControl<ProgressBar>("Progress");
            _operationType = this.FindControl<TextBlock>("OperationType");
            _viewModel = null;
            ProcessFinished = false;
        }
        public LoadingPopUp(int nbFiles, FileType modified, bool destroy=false) : this()
        {
            if (destroy)
                _operationType.Text = "Deleting ";
            else
                _operationType.Text = "Copying ";
            _operationType.Text += nbFiles + " files";
            _progress.Maximum = nbFiles;
            _viewModel = new LoadingPopUpViewModel(this, modified, nbFiles, destroy);
            DataContext = _viewModel;
            Task.Run(_viewModel.ReloadProgress);
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion
        

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            ProcessFinished = true;
        }
    }
}