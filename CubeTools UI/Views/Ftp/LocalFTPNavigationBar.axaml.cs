using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CubeTools_UI.Models;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library.Pointers;

namespace CubeTools_UI.Views.Ftp
{
    public class LocalFtpNavigationBar : UserControl
    {
        public static NavigationBarModel Model;
        public TextBox LocalCurrentPath;
        
        #region Init
        public LocalFtpNavigationBar()
        {
            InitializeComponent();
            LocalCurrentPath = this.FindControl<TextBox>("LocalCurrentPath");
            //Model = new NavigationBarModel(this);
            //DataContext = Model;
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
            if (Model.QueueIndex > 0)
            {
                Model.QueueIndex--;
                Model.ParentModel?.AccessPath(Model.QueuePointers[Model.QueueIndex].Path, true);
            }
        }

        /// <summary>
        /// The next pointer in the stack is chosen
        /// </summary>
        private void RightArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Model.QueueIndex < Model.QueuePointers.Count - 1)
            {
                Model.QueueIndex++;
                Model.ParentModel?.AccessPath(Model.QueuePointers[Model.QueueIndex].Path, true);
            }
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            if (Model.ParentModel.ModelActionBar.SelectedXaml[0].Pointer.Path != ManagerReader.GetRootPath(Model.ParentModel.ModelActionBar.SelectedXaml[0].Pointer.Path))
            {
                string path = Model.DirectoryPointer.Path;
                Model.ParentModel?.AccessPath(ManagerReader.GetParent(Model.QueuePointers[Model.QueueIndex].Path), true);
                Model.Add(new DirectoryType(path));
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
                    @managerException.Errorstd = "Unable to reload file";
                    new Views.ErrorPopUp.ErrorPopUp(@managerException).Show();
                }
            }
        }
        
        /// <summary>
        /// The settings is opened
        /// </summary>
        private void SettingsClick(object? sender, RoutedEventArgs e)
        {
            // TODO Implement a settings Window
        }
        
        #endregion
    }
}
