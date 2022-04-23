using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Views.PopUps
{
    public class LoadingPopUp : Window
    {
        private bool _destroy = false;
        private int _nbFiles = 0;
        private bool _processFinished;
        private ProgressBar _progress;
        private TextBlock _operationType;
        private FileType _modified;

        #region Init
        
        public LoadingPopUp()
        {
            InitializeComponent();
            _progress = this.FindControl<ProgressBar>("Progress");
            _operationType = this.FindControl<TextBlock>("OperationType");
        }
        public LoadingPopUp(int nbFiles, FileType modified, bool destroy=false) : this()
        {
            _nbFiles = nbFiles;
            _destroy = destroy;
            _modified = modified;
            if (_destroy)
                _operationType.Text = "Deleting ";
            else
                _operationType.Text = "Copying ";
            _operationType.Text += _nbFiles + " files";
            _progress.Maximum = nbFiles;
            ReloadProgress();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        #endregion

        public void ReloadProgress()
        {
            int value = 0;
            try
            {
                value = ManagerReader.FastReaderFiles(_modified.Path);
            }
            catch (Exception) {}
            Thread.Sleep(200);
        }


        private void OnClosing(object? sender, CancelEventArgs e)
        {
            _processFinished = true;
        }
    }
}