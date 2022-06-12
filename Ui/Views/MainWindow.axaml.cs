using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Ui.Views.ActionButtons;
using Ui.Views.Actions;
using Ui.Views.Error;
using Ui.Views.Ftp;
using Ui.Views.MenuController;
using Ui.Views.Settings;
using Menu = Ui.Views.MenuController.Menu;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class MainWindow : Window
    {
        public bool IsClosed;
        public static List<Key> KeysPressed;
        public LinkBar.LinkBar LinkBarView;
        public ClientUI LocalView;
        
        public MainWindow()
        {
            IsClosed = false;
            InitializeComponent();
            LinkBarView = this.FindControl<LinkBar.LinkBar>("LinkBar");
            LocalView = new ClientUI(new ClientLocal(), this);
            LocalView.ActionView.SetActionButtons(new List<ActionButton>
            {
                new CreateFileButton(LocalView, 0), new CreateFolderButton(LocalView, 1), new CopyButton(LocalView, 2),
                new CutButton(LocalView, 3), new PasteButton(LocalView, 4), new RenameButton(LocalView, 5),
                new CompressButton(LocalView, 6), new DeleteButton(LocalView, 7), new SortButton(LocalView, 9),
                new SearchButton(LocalView, 10), new SnapdropButton(LocalView, 11), new SmashButton(LocalView, 12)
            });
            this.FindControl<Grid>("ClientLocal").Children.Add(LocalView);
            // Grid grid = this.FindControl<Grid>("ClientLocal");
            // grid.Children.Add(new Ui.Views.MenuController.Menu());
            LinkBarView.Main = LocalView;
            LinkBarView.InitializeExpanders();
            KeysPressed = new List<Key>();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #region Events

        private void CheckEssentials()
        {
            if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["close"]))
                Close();
            else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["newWindow"]))
            {
                new MainWindow().Show();
                KeysPressed.Clear();
            }
            else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["settings"]))
            {
                new SettingsWindow(this).Show();
            }
            else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["openFTP"]))
            {
                new LoginFTP(LocalView).Show();
            }
            else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["openOneDrive"]))
            {
                ClientOneDrive clientRemote = new ClientOneDrive();
                ClientLocal clientLocal = new ClientLocal(Directory.GetCurrentDirectory().Replace('\\','/'));
                clientRemote.Client.authenticated += (o, success) =>
                {
                    if (success)
                    {
                        clientRemote.Children = clientRemote.ListChildren();
                        Dispatcher.UIThread.Post(() =>
                        {
                            var mainWindow = new MainWindowRemote(clientLocal, clientRemote);
                            mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
                            {
                                new CreateFileButton(mainWindow.RemoteView, 0), new CreateFolderButton(mainWindow.RemoteView, 1),
                                new CopyButton(mainWindow.RemoteView, 2), new CutButton(mainWindow.RemoteView, 3),
                                new PasteButton(mainWindow.RemoteView, 4), new RenameButton(mainWindow.RemoteView, 5),
                                new DeleteButton(mainWindow.RemoteView, 6), new DownloadButton(mainWindow.RemoteView, 7)
                            });
                            mainWindow.Show();
                        });
                    }
                    else new ErrorBase(new ConnectionRefused("OneDrive connection could not be established", "Connection to OneDrive")).Show();
                };
            }
            else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["openGoogleDrive"]))
            {
                ClientGoogleDrive clientRemote = new ClientGoogleDrive();
                ClientLocal clientLocal = new ClientLocal(Directory.GetCurrentDirectory().Replace('\\', '/'));
                var mainWindow = new MainWindowRemote(clientLocal, clientRemote);
                mainWindow.RemoteView.ActionView.SetActionButtons(new List<ActionButton>
                {
                    new CreateFileButton(mainWindow.RemoteView, 0), new CreateFolderButton(mainWindow.RemoteView, 1),
                    new CopyButton(mainWindow.RemoteView, 2), new CutButton(mainWindow.RemoteView, 3), new PasteButton(mainWindow.RemoteView, 4),
                    new RenameButton(mainWindow.RemoteView, 5), new DeleteButton(mainWindow.RemoteView, 6), new DownloadButton(mainWindow.RemoteView, 7)
                });
                mainWindow.Show();
            }
        }
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (KeysPressed.Contains(e.Key)) return;
            KeysPressed.Add(e.Key);
            if (IsListInListList(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts))
            {
                CheckEssentials();
                if (LocalView.subGrid.Children[0] is not Menu)
                {
                    if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createDir"]))
                    {
                        LocalView.ActionView.CreatDir(sender, e);
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createFile"]))
                    {
                        LocalView.ActionView.CreateFile(sender, e);
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["cut"]))
                    {
                        LocalView.ActionView.Cut(sender, e);
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["copy"]))
                    {
                        LocalView.ActionView.Copy(sender, e);
                    }
                    else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["delete"]))
                    {
                        Thread deleteThread = new Thread(() =>
                        {
                            foreach (PointerItem item in LocalView.ActionView.SelectedXaml)
                            {
                                try
                                {
                                    LocalView.Client.Delete(item.Pointer);
                                }
                                catch (Exception exception)
                                {
                                    if (exception is ManagerException @managerException)
                                    {
                                        @managerException.Errorstd = $"Unable to delete {item.Pointer.Name}";
                                        new ErrorBase(@managerException).ShowDialog<object>(this);
                                    }
                                }
                            }
                            LocalView.ActionView.SelectedXaml.Clear();
                            Dispatcher.UIThread.Post(() => LocalView.Refresh(), DispatcherPriority.Render);
                        });
                        deleteThread.Start();
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["paste"]))
                    {
                        LocalView.ActionView.Paste(sender, e);
                        LocalView.ActionView.SelectedXaml.Clear();
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["search"]))
                    {
                        LocalView.ActionView.Search(sender, e);
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["selectAll"]))
                    {
                        // All items are selected
                        if (LocalView.ActionView.SelectedXaml.Count == LocalView.PointersView.Generator.Children.Count)
                        {
                            LocalView.ActionView.SelectedXaml.Clear();
                            int size = LocalView.PointersView.Generator.Children.Count;
                            for (int i = 0; i < size; i++)
                            {
                                LocalView.ActionView.SelectedXaml.Add((PointerItem) LocalView.PointersView.Generator.Children[i]);
                                size = LocalView.PointersView.Generator.Children.Count;
                                foreach (var control in LocalView.ActionView.SelectedXaml)
                                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                            }
                        }
                        else
                        {
                            int size = LocalView.PointersView.Generator.Children.Count;
                            for (int i = 0; i < size; i++)
                            {
                                if (!LocalView.ActionView.SelectedXaml.Contains((PointerItem) LocalView.PointersView.Generator.Children[i]))
                                    LocalView.ActionView.SelectedXaml.Add((PointerItem) LocalView.PointersView.Generator.Children[i]);
                                size = LocalView.PointersView.Generator.Children.Count;
                                foreach (var control in LocalView.ActionView.SelectedXaml)
                                    control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                            }
                            
                        }
                    }
                    else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["rename"]))
                    {
                        if (LocalView.ActionView.SelectedXaml.Count == 1)
                        {
                            new Rename(LocalView.ActionView.SelectedXaml[0].Pointer, LocalView.Client.Children, LocalView).Show();
                        }
                    }
                    else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["reload"]))
                    {
                        LocalView.Refresh();
                    }
                    else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["compress"]))
                    {
                        List<Pointer> pointers = new List<Pointer>();
                        foreach (PointerItem item in LocalView.ActionView.SelectedXaml)
                        {
                            pointers.Add(item.Pointer);
                        }
                        new Compress(LocalView, pointers).Show();
                    }
                    else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["sort"]))
                    {
                        new Sort(LocalView).Show();
                    }
                }
            }
        }

        public void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            KeysPressed.Remove(e.Key);
        }
        
        #endregion
        
        #region Process

        private static bool IsListInListList(List<Key> list, Dictionary<string, List<Key>> listList)
        {
            foreach (var (name, list2) in listList)
            {
                if (list2.Count == list.Count)
                {
                    bool stop = false;
                    int i = 0;
                    while (i < list2.Count)
                    {
                        if (list[i] != list2[i])
                            stop = true;
                        i++;
                    }
                    if (!stop) return true;
                }
            }
            return false;
        }

        private static bool AreListsEqual(List<Key> list, List<Key> list2)
        {
            if (list.Count != list2.Count) return false;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != list2[i]) return false;
            }
            return true;
        }
        
        #endregion


        private void Closed(object? sender, CancelEventArgs e)
        {
            IsClosed = true;
        }
    }
}