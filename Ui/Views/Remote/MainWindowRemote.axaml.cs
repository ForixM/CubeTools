using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library.ManagerExceptions;
using Library.ManagerReader;
using LibraryClient;
using Ui.Views.Settings;

namespace Ui.Views.Remote
{
    public class MainWindowRemote : Window
    {
        #region Variables
        
        public static MainWindowRemote LastView;

        public Local.Local Local;
        
        public RemoteNavigation RemoteNavigationView;
        public RemoteAction RemoteActionView;
        public RemotePointers RemotePointersView;

        public Client Client;
        public List<Key> KeysPressed;

        #endregion

        #region Init
        
        public MainWindowRemote()
        {
            LastView = this;
            InitializeComponent();
            // References
            Local = this.FindControl<Local.Local>("Local");
            RemoteNavigationView = this.FindControl<RemoteNavigation>("RemoteNavigation");
            RemoteActionView = this.FindControl<RemoteAction>("RemoteAction");
            RemotePointersView = this.FindControl<RemotePointers>("RemotePointers");
            // Components
            KeysPressed = new List<Key>();
        }

        public MainWindowRemote(Client client) : this()
        {
            Client = client;
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #endregion

        #region Process

        public void ReloadPath()
        {
            try
            {
                Client!.Refresh();
                
            }
            catch (Exception e)
            {
                if (e is ManagerException @managerException) new Views.ErrorPopUp.ErrorPopUp();
            }
        }

        public void AccessPath(RemoteItem item, bool isdir)
        {
            if (Client is null) return;
            
            if (isdir)
            {
                Client.AccessPath(item);
            }
            else
            {
                try
                {
                    ManagerReader.AutoLaunchAppProcess(Client.Download(item, Local.NavigationBarView.FolderPointer.Path)
                        .Path);
                }
                catch (Exception e)
                {
                    if (e is ManagerException managerException) SelectErrorPopUp(managerException);
                }
            }
        }
        
        public void AccessPath(string name, bool isdir)
        {
            RemoteItem? item = Client?.GetItem(name);
            if (item is not null) AccessPath(item, isdir);
        }
        
        public void SelectErrorPopUp(ManagerException exception) => Local.SelectErrorPopUp(exception);

        #endregion
        
        #region Events
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            if (KeysPressed.Contains(e.Key)) return;
            KeysPressed.Add(e.Key);
            if (IsListInListList(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.Shortcuts))
            {
                if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CloseShortCut))
                    Close();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CopyShortCut))
                    RemoteActionView.Copy(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CreateDirShortcut))
                    RemoteActionView.CreatDir(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CreateFileShortcut))
                    RemoteActionView.CreateFile(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CutShortCut))
                    RemoteActionView.Cut(sender, e);
                else if (KeysPressed == ConfigLoader.ConfigLoader.Settings.Shortcuts.DeleteShortCut)
                    RemoteActionView.Delete(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.PasteShortCut))
                    RemoteActionView.Paste(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.SelectAllShortcut))
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
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts.NewWindowShortCut))
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
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.RenameShortcut))
                    RemoteActionView.Rename(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.ReloadShortcut))
                    ReloadPath();
                else if (KeysPressed ==ConfigLoader.ConfigLoader.Settings.Shortcuts.SettingsShortcut)
                    new SettingsWindow().Show();
            }
        }
        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            while (KeysPressed.Count != 0 && KeysPressed.Last() != e.Key)
                KeysPressed.RemoveAt(KeysPressed.Count - 1);
        }
        
        #endregion
        
        #region Process

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