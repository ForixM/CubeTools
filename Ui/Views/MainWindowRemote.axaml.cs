using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using DynamicData;
using Library;
using Library.ManagerExceptions;
using Ui.Views.ActionButtons;
using Ui.Views.Actions;
using Ui.Views.Error;
using Ui.Views.Ftp;
using Ui.Views.Settings;
using Pointer = Library.Pointer;

namespace Ui.Views
{
    public class MainWindowRemote : Window
    {
        public bool IsClosed;
        public List<Key> KeysPressed;
        public LinkBar.LinkBar LinkBarView;
        public ClientUI LocalView;
        public ClientUI RemoteView;
        public bool localFocused = true;

        public MainWindowRemote()
        {
            InitializeComponent();
            LinkBarView = this.FindControl<LinkBar.LinkBar>("LinkBar");
            // Local Client
            LocalView = new ClientUI(new ClientLocal(), this);
            this.FindControl<Grid>("ClientLocal").Children.Add(LocalView);
            LinkBarView.Main = LocalView;
            // Initialize variables
            LinkBarView.InitializeExpanders();
            KeysPressed = new List<Key>();
            IsClosed = false;
        }

        public MainWindowRemote(Client clientLocal, Client clientRemote) : this()
        {
            InitializeComponent();
            
            // Local Client
            LocalView = new ClientUI(clientLocal, this);
            this.FindControl<Grid>("ClientLocal").Children.Add(LocalView);
            LocalView.ActionView.SetActionButtons(new List<ActionButton>
            {
                new CreateFileButton(LocalView, 0), new CreateFolderButton(LocalView, 1), new CopyButton(LocalView, 2), new CutButton(LocalView, 3), new PasteButton(LocalView, 4),
                new RenameButton(LocalView, 5), new CompressButton(LocalView, 6), new DeleteButton(LocalView, 7), new UploadButton(LocalView, 8)
            });
            
            // Link Bar
            LinkBarView = this.FindControl<LinkBar.LinkBar>("LinkBar");
            LinkBarView.Main = LocalView;
            LinkBarView.stackPanel.Children.RemoveAt(0);
            
            
            // Remote Client
            RemoteView = new ClientUI(clientRemote, this);
            this.FindControl<Grid>("ClientRemote").Children.Add(RemoteView);
            
            LinkBarView.ChangeLinkBarIcon();
            
            // Initialize variables
            LinkBarView.InitializeExpanders();
            KeysPressed = new List<Key>();
            IsClosed = false;
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
                ClientUI selected = localFocused ? LocalView : RemoteView;
                if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createDir"]))
                {
                    selected.ActionView.CreatDir(sender, e);
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createFile"]))
                {
                    selected.ActionView.CreateFile(sender, e);
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["cut"]))
                {
                    selected.ActionView.Cut(sender, e);
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["copy"]))
                {
                    selected.ActionView.Copy(sender, e);
                }
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["delete"]))
                {
                    Thread deleteThread = new Thread(() =>
                    {
                        foreach (PointerItem item in selected.ActionView.SelectedXaml)
                        {
                            try
                            {
                                selected.Client.Delete(item.Pointer);
                                // if (LocalView.ActionView.SelectedXaml.Count != 0)
                                //     LocalView.Client.Delete(item.Pointer);
                                // else
                                //     RemoteView.Client.Delete(item.Pointer);
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
                        selected.ActionView.SelectedXaml.Clear();
                        // LocalView.ActionView.SelectedXaml.Clear();
                        // RemoteView.ActionView.SelectedXaml.Clear();
                        Dispatcher.UIThread.Post(() =>
                        {
                            selected.Refresh();
                            // if (LocalView.ActionView.SelectedXaml.Count != 0)
                            //     LocalView.Refresh();
                            // else
                            //     RemoteView.Refresh();
                        }, DispatcherPriority.Render);
                    });
                    deleteThread.Start();
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["paste"]))
                {
                        selected.ActionView.Paste(sender, e);
                        selected.ActionView.SelectedXaml.Clear();
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["search"]))
                {
                    selected.ActionView.Search(sender, e);
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["selectAll"]))
                {
                    // All items are selected
                    if (selected.ActionView.SelectedXaml.Count == selected.PointersView.Generator.Children.Count)
                    {
                        selected.ActionView.SelectedXaml.Clear();
                        int size = selected.PointersView.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            selected.ActionView.SelectedXaml.Add((PointerItem) selected.PointersView.Generator.Children[i]);
                            size = selected.PointersView.Generator.Children.Count;
                            foreach (var control in selected.ActionView.SelectedXaml)
                                control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                        }
                    }
                    else
                    {
                        int size = selected.PointersView.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            if (!selected.ActionView.SelectedXaml.Contains((PointerItem) selected.PointersView.Generator.Children[i]))
                                selected.ActionView.SelectedXaml.Add((PointerItem) selected.PointersView.Generator.Children[i]);
                            size = selected.PointersView.Generator.Children.Count;
                            foreach (var control in selected.ActionView.SelectedXaml)
                                control.button.Background = new SolidColorBrush(new Color(255, 255, 224, 130));
                        }
                        
                    }
                }
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["rename"]))
                {
                    if (selected.ActionView.SelectedXaml.Count == 1)
                    {
                        new Rename(selected.ActionView.SelectedXaml[0].Pointer, selected.Client.Children, selected).Show();
                    }
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["reload"]))
                {
                    selected.Refresh();
                }
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["compress"]))
                {
                    List<Pointer> pointers = new List<Pointer>();
                    foreach (PointerItem item in selected.ActionView.SelectedXaml)
                    {
                        pointers.Add(item.Pointer);
                    }
                    new Compress(selected, pointers).Show();
                }
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["sort"]))
                {
                    new Sort(selected).Show();
                }
            }
        }
        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e) => KeysPressed.Remove(e.Key);
        
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