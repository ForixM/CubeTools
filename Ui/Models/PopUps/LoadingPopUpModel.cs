using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Library.ManagerReader;
using Library;
using ReactiveUI;
using Ui.Views.PopUps;

namespace Ui.Models.PopUps
{
    public class LoadingPopUpModel : ReactiveObject
    {
        private LoadingPopUp _main;
        private ProgressBar _progressBar;
        
        #region Process Variables
        
        private IProgress<int> _progress;
        private double _max;
        private bool _destroy;
        private List<FilePointer> _modified;

        #endregion
    
        public LoadingPopUpModel()
        {
            _modified = new List<FilePointer>();
            _max = 100;
        }
        public LoadingPopUpModel(LoadingPopUp main, List<FilePointer> modified, double max, bool destroy, ProgressBar progressBar)
        {
            _main = main;
            _modified = modified;
            _max = max;
            _destroy = destroy;
            _progressBar = progressBar;
            _progress = new Progress<int>(percent =>
            {
                _progressBar.Value = percent;
            });
            StartUpdating();
        }

        private void StartUpdating()
        {
            Task.Run(() =>
            {
                while (Math.Abs(_max - 100) > 2 && _main.IsActive)
                    ReloadProgress().Start();
            });
        }
        
        private Task ReloadProgress()
        {
            return new Task(() =>
            {
                if (_destroy)
                {
                    long sum = _modified.Sum(ft => ManagerReader.FastReaderFiles(ft.Path));
                    _progressBar.Value = ((int)((_max - sum) / _max * 100));
                    Thread.Sleep(500);
                }
                else
                {
                    long sum = _modified.Sum(ft => ManagerReader.FastReaderFiles(ft.Path));
                    _progressBar.Value = ((int) (sum / _max * 100));
                    Thread.Sleep(500);
                }
            });
        }
    }
}
