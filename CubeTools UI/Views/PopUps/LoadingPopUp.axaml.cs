using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models.PopUps;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class LoadingPopUp : Window
    {
        public bool ProcessFinished;
        private ProgressBar _progressBar;
        private TextBlock _operationType;
        private LoadingPopUpModel _viewModel;
        
        #region Init
        
        public LoadingPopUp()
        {
            InitializeComponent();
            _progressBar = this.FindControl<ProgressBar>("ProgressBar");
            _operationType = this.FindControl<TextBlock>("OperationType");
            _viewModel = null;
            ProcessFinished = false;
        }
        public LoadingPopUp(int nbFiles, List<FileType> modified, bool destroy=false) : this()
        {
            if (destroy) _operationType.Text = "Deleting ";
            else _operationType.Text = "Copying ";
            
            _operationType.Text += nbFiles + " files";
            _viewModel = new LoadingPopUpModel(this, modified, nbFiles, destroy, _progressBar);
        }
        
        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        private void OnClosing(object? sender, CancelEventArgs e)
        {
            ProcessFinished = true;
        }
    }
}