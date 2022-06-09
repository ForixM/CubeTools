using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerExceptions;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.Error;
using Ui.Views.Actions;

namespace Ui.Views
{
    public class ActionView : UserControl
    {
        public List<PointerItem> SelectedXaml;
        public List<PointerItem> CopiedXaml;
        public List<PointerItem> CutXaml;

        public ClientUI Main;

        public WrapPanel generator;

        #region Init
        
        public ActionView()
        {
            InitializeComponent();
            Main = ClientUI.LastReference;
            SelectedXaml = new List<PointerItem>();
            CopiedXaml = new List<PointerItem>();
            CutXaml = new List<PointerItem>();
            generator = this.FindControl<WrapPanel>("Generator");
            generator.ItemWidth = 45;
            generator.Children.Add(new CreateFileButton());
            generator.Children.Add(new CreateFolderButton());
            generator.Children.Add(new RenameButton());
            generator.Children.Add(new DeleteButton());
        }
        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);
        
        #endregion

        #region Events

        /// <summary>
        /// Create a new file (New File by default)
        /// </summary>
        public void CreateFile(object? sender, RoutedEventArgs e) => new CreateFile(Main).Show();

        /// <summary>
        /// Create a new directory (New Folder by default)
        /// </summary>
        public void CreatDir(object? sender, RoutedEventArgs e) => new CreateFolder(Main).Show();

        /// <summary>
        /// Add to Copied the selected elements
        /// </summary>
        public void Copy(object? sender, RoutedEventArgs e)
        {
            CopiedXaml.Clear();
            CutXaml.Clear();
            foreach (var item in SelectedXaml) CopiedXaml.Add(item);
        }

        /// <summary>
        /// Add to cut the selected elements
        /// </summary>
        public void Cut(object? sender, RoutedEventArgs e)
        {
            CopiedXaml.Clear();
            CutXaml.Clear();
            foreach (var item in SelectedXaml)
            {
                CopiedXaml.Add(item);
                CutXaml.Add(item);
            }
        }

        /// <summary>
        /// Paste copied and delete cut
        /// </summary>
        public void Paste(object? sender, RoutedEventArgs e)
        {
            // 1) Copy Copied
            foreach (var item in CopiedXaml)
                CopyPointer(item.Pointer);

            switch (CutXaml.Count)
            {
                case 0:
                    return;
                case 1:
                    new Delete(Main, CutXaml[0].Pointer).Show();
                    break;
                default:
                    new DeleteMultiple(Main, CutXaml.Select(pointer => pointer.Pointer).ToList()).Show();
                    break;
            }
        }

        /// <summary>
        /// Rename a file or directory
        /// </summary>
        public void Rename(object? sender, RoutedEventArgs e)
        {
            switch (SelectedXaml.Count)
            {
                case < 1:
                    return;
                case 1:
                    new Rename(SelectedXaml[0].Pointer, Main.Client.Children, Main).Show();
                    break;
                default:
                    new ErrorBase(new ManagerException("Unable to rename multiple data")).ShowDialog<object>(Main.Main);
                    break;
            }
        }

        /// <summary>
        /// Delete selected
        /// </summary>
        public void Delete(object? sender, RoutedEventArgs e)
        {
            switch (SelectedXaml.Count)
            {
                case 0:
                    return;
                case 1:
                    new Delete(Main, SelectedXaml[0].Pointer).Show();
                    break;
                default:
                    new DeleteMultiple(Main, Main.ActionView.SelectedXaml.Select(pointer => pointer.Pointer).ToList()).Show();
                    break;
            }
        }

        /// <summary>
        /// Display the search box
        /// </summary>
        public void Search(object? sender, RoutedEventArgs e) => new Search(Main).Show();

        /// <summary>
        /// Display the sort box
        /// </summary>
        public void Sort(object? sender, RoutedEventArgs e) => new Sort(Main).Show();
        
        /// <summary>
        /// Open the Snap Drop application in a browser
        /// </summary>
        private void Snap(object? sender, RoutedEventArgs e)
        {
            var uri = "https://www.snapdrop.net";
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }
        
        /// <summary>
        /// Open the smash application in a browser
        /// </summary>
        private void Smash(object? sender, RoutedEventArgs e)
        {
            const string uri = "https://www.fromsmash.com";
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = uri
            };
            System.Diagnostics.Process.Start(psi);
        }

        /// <summary>
        /// Compression selected files and folders by displaying folder and files popup for compression
        /// </summary>
        private void Compression(object? sender, RoutedEventArgs e)
        {
            if (SelectedXaml.Count == 0) return;

            var archives = SelectedXaml.Where(ft =>  ft.Pointer.Type is "zip" or "7z").Select(item => item.Pointer);
            var others = SelectedXaml.Where(ft =>  ft.Pointer.Type is not "zip" && ft.Pointer.Type is not "7z").Select(item => item.Pointer);

            // Opening extract for archives
            var archiveTypes = archives.ToList();
            if (archiveTypes.Any()) new Extract(Main, archiveTypes).Show();
            
            // Extracting all archives
            var allTypes = others.ToList();
            if (allTypes.Any()) new Compress(Main, allTypes).Show();
        }
        
        #endregion

        #region Process
     
        /// <summary>
        /// Copy the given pointer
        /// </summary>
        /// <param name="source">The pointer that we want to make a copy</param>
        private void CopyPointer(Pointer source)
        {
            try
            {
                // Copy Pointer
                Dispatcher.UIThread.Post(
                    () =>
                    {
                        Main.Client.Copy(source);
                    },
                    DispatcherPriority.MaxValue);
            }
            catch (Exception exception)
            {
                if (exception is ManagerException @managerException)
                {
                    @managerException.Errorstd = $"Unable to copy {source.Name}";
                    new ErrorBase(@managerException).ShowDialog<object>(Main.Main);
                }
            }
        }

        #endregion
    }
}
