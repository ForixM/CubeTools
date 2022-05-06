using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using CubeTools_UI.Views.Settings;
using Library.ManagerExceptions;
using Library.ManagerReader;

namespace CubeTools_UI.Views
{
    public class NavigationBar : UserControl
    {
        public readonly NavigationBarModel Model;
        public static NavigationBarModel? LastModel;
        public TextBox CurrentPathXaml;
        
        #region Init
        public NavigationBar()
        {
            InitializeComponent();
            CurrentPathXaml = this.FindControl<TextBox>("CurrentPath");
            Model = new NavigationBarModel(this);
            LastModel = Model;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion
        
        #region Events
        
        private void EditCurrentPath(object? sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            Model.ParentModel?.AccessPath(((TextBox) sender!).Text);
        }

        /// <summary>
        /// The last pointer is chosen
        /// </summary>
        private void LeftArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Model.QueueIndex < Model.QueuePointers.Count - 1 && Model.ParentModel != null)
            {
                Model.QueueIndex++;
                try
                {
                    Model.ParentModel?.AccessPath(Model.QueuePointers[Model.QueueIndex]);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to get the next file";
                        Model.ParentModel!.SelectErrorPopUp(@managerException);
                    }
                    Model.QueueIndex--;
                }
            }
        }

        /// <summary>
        /// The next pointer in the stack is chosen
        /// </summary>
        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            // End of the queue
            if (Model.QueueIndex > 0 && Model.ParentModel != null)
            {
                // Get the index before
                Model.QueueIndex--;
                try
                {
                    string path = Model.QueuePointers[Model.QueueIndex];
                    Model.ParentModel?.AccessPath(path);
                }
                catch (Exception exception)
                {
                    if (exception is ManagerException @managerException)
                    {
                        @managerException.Errorstd = "Unable to get the last file";
                        Model.ParentModel!.SelectErrorPopUp(@managerException);
                    }
                    Model.QueueIndex--;
                }
            }
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            string parent = "";
            try
            {
                if (ManagerReader.GetRootPath(Model.DirectoryPointer.Path) == Model.DirectoryPointer.Path)
                    parent = Model.DirectoryPointer.Path;
                else 
                    parent = ManagerReader.GetParent(Model.DirectoryPointer.Path);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to get the parent file";
                    Model.ParentModel!.SelectErrorPopUp(@managerException);
                }
            }
            Model.QueuePointers.Add(parent);
            Model.QueueIndex = Model.QueuePointers.Count-1;
            try
            {
                Model.ParentModel?.AccessPath(Model.QueuePointers[Model.QueueIndex]);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to get the parent";
                    Model.ParentModel!.SelectErrorPopUp(@managerException);
                }
            }
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Model.DirectoryPointer.SetChildrenFiles();
                Model.ParentModel!.ReloadPath();
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = "Unable to reload files and folders";
                    Model.ParentModel!.SelectErrorPopUp(@managerException);
                }
            }
        }
        
        /// <summary>
        /// The settings is opened
        /// </summary>
        private void SettingsClick(object? sender, RoutedEventArgs e) => new SettingsWindow().Show();
        
        #endregion
    }
}
