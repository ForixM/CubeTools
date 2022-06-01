using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library;
using Library.DirectoryPointer.DirectoryPointerLoaded;
using Ui.Models;
using Ui.Views.Settings;

namespace Ui.Views
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
            // End of the queue
        }

        /// <summary>
        /// The parent is being selected
        /// </summary>
        private void UpArrowClick(object? sender, RoutedEventArgs e)
        {
            Model.ParentModel?.AccessPath(ManagerReader.GetParent(Model.FolderPointer.Path));
            Model.Add(new DirectoryPointerLoaded(Model.FolderPointer.Path));
        }

        /// <summary>
        /// The sync is being pressed
        /// </summary>
        public void SyncClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                Model.FolderPointer.SetChildrenFiles();
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
