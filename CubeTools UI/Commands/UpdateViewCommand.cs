using CubeTools_UI.Models;
using System;
using System.Windows.Input;

namespace CubeTools_UI.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private MainWindowModel _model;

        public UpdateViewCommand(MainWindowModel model)
        {
            this._model = model;
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
                //model.SelectedViewModel = new PathsBarModel();
            }
            else if(parameter.ToString() == "GoogleDrive")
            {
                //model.SelectedViewModel = new GoogleDriveFileModel();
            }
            else if(parameter.ToString() == "OneDrive")
            {
                //model.SelectedViewModel = new OneDriveModel();
            }
        }
    }
}