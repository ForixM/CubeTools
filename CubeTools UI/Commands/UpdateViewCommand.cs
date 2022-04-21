using CubeTools_UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CubeTools_UI.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainWindowViewModel viewModel;

        public UpdateViewCommand(MainWindowViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(parameter.ToString() == "Path")
            {
                viewModel.SelectedViewModel = new PathsBarViewModel();
            }
            else if(parameter.ToString() == "GoogleDrive")
            {
                viewModel.SelectedViewModel = new GoogleDriveFileModelView();
            }
            else if(parameter.ToString() == "OneDrive")
            {
                viewModel.SelectedViewModel = new OneDriveModelView();
            }
        }
    }
}