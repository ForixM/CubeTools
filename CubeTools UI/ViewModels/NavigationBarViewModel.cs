using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Data;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.ManagerReader;
using ReactiveUI;

namespace CubeTools_UI.ViewModels
{
    public class NavigationBarViewModel : ReactiveObject
    {
        // Pointers
        private MainWindowModel _model;
        public MainWindowModel ModelXaml
        {
            get => _model;
            set => _model = value;
        }

        private NavigationBarModel _modelNavigationBar;
        public NavigationBarModel ModelNavigationBarXaml
        {
            get => _modelNavigationBar;
            set => _modelNavigationBar = value;
        }

        private MainWindowViewModel _parentParentViewModel;
        public MainWindowViewModel ParentViewModelXaml
        {
            get => _parentParentViewModel;
            set => _parentParentViewModel = value;
        }

        // CTOR
        public NavigationBarViewModel()
        {
            _model = null;
            _modelNavigationBar = null;
            _parentParentViewModel = null;
        }
        
        public string CurrentPath
        {
            get => _parentParentViewModel is null ? Directory.GetCurrentDirectory() : _parentParentViewModel.Model.ModelNavigationBar.DirectoryPointer.Path;
            set
            {
                if (Directory.Exists(value))
                {
                    if (value != _parentParentViewModel.Model.ModelNavigationBar.DirectoryPointer.Path)
                    {
                        _parentParentViewModel.AccessPath(value);
                    }

                    this.RaiseAndSetIfChanged(ref value, value);
                }
                else 
                    _parentParentViewModel.ErrorMessageBox(new PathNotFoundException("Unable to access the directory"), $"Directory {value} not found");
            }
        }

        // All functions are called in XAML code

        /// <summary>
        ///  Binding method : Get the last directory loaded in the queue
        /// </summary>
        public void LeftBtnClick()
        {
            // End of the queue
            if (_modelNavigationBar.QueueIndex > 0)
            {
                // Get the index before
                _modelNavigationBar.QueueIndex--;
                try
                {
                    string path = _modelNavigationBar.QueuePointers[_modelNavigationBar.QueueIndex];
                    _model.ViewModel.AccessPath(path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException @managerException)
                        ParentViewModelXaml.ErrorMessageBox(@managerException, $"Unable to get the last directory");
                    _modelNavigationBar.QueueIndex--;
                }
            }
        }

        /// <summary>
        /// Binding Method : Get next directory loaded in the queue
        /// </summary>
        public void RightBtnClick()
        {
            if (_modelNavigationBar.QueueIndex < _modelNavigationBar.QueuePointers.Count - 1)
            {
                _modelNavigationBar.QueueIndex++;
                try
                {
                    _model.ViewModel.AccessPath(_modelNavigationBar.QueuePointers[_modelNavigationBar.QueueIndex]);
                }
                catch (Exception e)
                {
                    if (e is ManagerException @managerException)
                        ParentViewModelXaml.ErrorMessageBox(@managerException, $"Unable to get the next directory");
                    _modelNavigationBar.QueueIndex--;
                }
            }
        }

        /// <summary>
        /// Binding Method : get the parent directory of the current loaded directory
        /// </summary>
        public void UpBtnClick()
        {
            string parent = "";
            try
            {
                if (ManagerReader.GetRootPath(_modelNavigationBar.DirectoryPointer.Path) ==
                    _modelNavigationBar.DirectoryPointer.Path)
                    parent = _modelNavigationBar.DirectoryPointer.Path;
                else 
                    parent = ManagerReader.GetParent(_modelNavigationBar.DirectoryPointer.Path);
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException)
                    ParentViewModelXaml.ErrorMessageBox(@managerException, $"Unable to get directory of ");
                else
                    throw;
            }
            
            _modelNavigationBar.QueuePointers.Add(parent);
            _modelNavigationBar.QueueIndex = _modelNavigationBar.QueuePointers.Count-1;
            try
            {
                _model.ViewModel.AccessPath(_modelNavigationBar.QueuePointers[_modelNavigationBar.QueueIndex]);
            }
            catch (Exception e)
            {
                if ( e is ManagerException @managerException)
                    ParentViewModelXaml.ErrorMessageBox(@managerException, "Unable to get parent");
            }
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void SyncBtnClick()
        {
            // TODO Add OneDrive implementation
        }

        /// <summary>
        ///     - Action : <br></br>
        ///     - XAML : <br></br>
        ///     - Implementation :
        /// </summary>
        public void SettingBtnClick()
        {
            // TODO Implement setting View
        }
    }
}
