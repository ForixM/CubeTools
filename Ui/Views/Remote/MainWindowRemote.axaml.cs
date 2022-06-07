using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Library.ManagerExceptions;
using Library.ManagerReader;
using Library;
using Library.LibraryOneDrive;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Ui.Views.Error;
using Ui.Views.Settings;
using Pointer = Library.Pointer;

namespace Ui.Views.Remote
{
    public class MainWindowRemote : Window
    {
        #region Variables
        
        public static MainWindowRemote LastReference;

        // public Local.Local Local;
        
        public RemoteNavigation RemoteNavigationView;
        public RemoteAction RemoteActionView;
        public RemotePointers RemotePointersView;

        public Client Local;
        public Client Client;
        public List<Key> KeysPressed;
        public bool GotFocusLocal;

        #endregion

        #region Init
        
        public MainWindowRemote()
        {
            LastReference = this;
            InitializeComponent();
            // References
            // Local = this.FindControl<Local.Local>("Local");
            // Local.IsRemote = true;
            RemoteNavigationView = this.FindControl<RemoteNavigation>("RemoteNavigation");
            RemoteActionView = this.FindControl<RemoteAction>("RemoteAction");
            RemotePointersView = this.FindControl<RemotePointers>("RemotePointers");
            // Variables
            KeysPressed = new List<Key>();
            GotFocusLocal = true;
        }

        public MainWindowRemote(Client client, Client local) : this()
        {
            Client = client;
            this.Local = local;
            RemoteNavigationView.CurrentPathXaml.Text = client.CurrentFolder.Path;
            RemoteNavigationView.Add(Client.CurrentFolder!);
            Refresh();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #endregion

        #region Process

        public void Refresh()
        {
            try
            {
                Client.Refresh();
                RemotePointersView.Refresh();
                RemoteNavigationView.Refresh();
            }
            catch (Exception e)
            {
                if (e is ManagerException managerException) new ErrorBase(managerException).ShowDialog<object>(this);
            }
        }

        /// <summary>
        /// Access to an item
        /// </summary>
        /// <param name="pointer">the given item to access</param>
        public void AccessPath(Pointer pointer)
        {
            if (pointer.IsDir)
            {
                try
                {
                    Client.AccessPath(pointer);
                    RemotePointersView.Refresh();
                    RemoteNavigationView.Refresh();
                }
                catch (Exception e)
                {
                
                    if (e is ManagerException managerException) new ErrorBase(managerException).ShowDialog<object>(this);
                }
            }
            else
            {
                try
                {
                    ManagerReader.AutoLaunchAppProcess(Client.DownloadFile(pointer, Local.NavigationBarView.FolderPointer).Path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException managerException) new ErrorBase(managerException).ShowDialog<object>(this);
                }
            }
        }

        public void AccessPath(string path)
        {
            var item = Client.GetItem(path, true);
            if (item is not null) AccessPath(item);
            else new ErrorBase(new PathNotFoundException("Unable to access the path","AccessPath")).ShowDialog<object>(this);
        }

        #endregion
        
        #region Events
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (KeysPressed.Contains(e.Key)) return;
            KeysPressed.Add(e.Key);
            if (IsListInListList(KeysPressed,ConfigLoader.ConfigLoader.Settings.ListShortcuts))
            {
                if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["close"]))
                    Close();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["copy"]))
                    RemoteActionView.Copy(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createDir"]))
                    RemoteActionView.CreatDir(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createFile"]))
                    RemoteActionView.CreateFile(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["cut"]))
                    RemoteActionView.Cut(sender, e);
                else if (KeysPressed == ConfigLoader.ConfigLoader.Settings.Shortcuts["delete"])
                    RemoteActionView.Delete(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["paste"]))
                    RemoteActionView.Paste(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["selectAll"]))
                {
                    // All items are selected
                    if (RemoteActionView.Selected.Count == RemotePointersView.Generator.Children.Count)
                    {
                        RemoteActionView.Selected.Clear();
                        int size = RemotePointersView.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            RemoteActionView.Selected.Add(((RemotePointer) RemotePointersView.Generator.Children[i]).Pointer);
                            size = RemotePointersView.Generator.Children.Count;
                        }
                    }
                    else
                    {
                        int size =  RemotePointersView.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            if (!RemoteActionView.Selected.Contains( ( (RemotePointer) RemotePointersView.Generator.Children[i]).Pointer ) )
                                RemoteActionView.Selected.Add(((RemotePointer)  RemotePointersView.Generator.Children[i]).Pointer);
                            size =  RemotePointersView.Generator.Children.Count;
                        }
                        
                    }
                }
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["newWindow"]))
                {
                    var remote = new MainWindowRemote();
                    Client client;
                    switch (Client.Type)
                    {
                        case ClientType.FTP :
                            var castFTP = (ClientTransferProtocol) Client;
                            client = new ClientTransferProtocol(ClientType.FTP, castFTP.Host, castFTP.Username,
                                castFTP.Password);
                            break;
                        case ClientType.ONEDRIVE :
                            var castOD = (ClientOneDrive) Client;
                            client = new ClientOneDrive(ClientType.ONEDRIVE);
                            break;
                        default:
                            var castGD = (ClientGoogleDrive) Client;
                            client = new ClientGoogleDrive(ClientType.GOOGLEDRIVE);
                            break;
                    }
                    remote.Client = client;
                    remote.Show();
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["rename"]))
                    RemoteActionView.Rename(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["reload"]))
                    Refresh();
                else if (KeysPressed == ConfigLoader.ConfigLoader.Settings.Shortcuts["settings"])
                    new SettingsWindow().Show();
            }
        }
        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            while (KeysPressed.Count != 0 && KeysPressed.Last() != e.Key)
                KeysPressed.RemoveAt(KeysPressed.Count - 1);
        }
        
        #endregion
        
        #region Appendix

        private static bool IsListInListList(List<Key> list, List<List<Key>> listList)
        {
            foreach (var list2 in listList)
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
    }
}