using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using DynamicData;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.Settings;

namespace Ui.Views
{
    public class MainWindowRemote : Window
    {
        public bool IsClosed;
        public List<Key> KeysPressed;
        public LinkBar.LinkBar LinkBarView;
        public ClientUI LocalView;
        public ClientUI RemoteView;

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
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            
            if (KeysPressed.Contains(e.Key)) return;
            KeysPressed.Add(e.Key);
            if (IsListInListList(KeysPressed,ConfigLoader.ConfigLoader.Settings.ListShortcuts))
            {
                if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["close"]))
                    Close();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createDir"]))
                    LocalView.ActionView.CreatDir(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createFile"]))
                    LocalView.ActionView.CreateFile(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["cut"]))
                    LocalView.ActionView.Cut(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["copy"]))
                    LocalView.ActionView.Copy(sender, e);
                else if (KeysPressed == ConfigLoader.ConfigLoader.Settings.Shortcuts["delete"])
                    LocalView.ActionView.Delete(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["paste"]))
                    LocalView.ActionView.Paste(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["search"]))
                    LocalView.ActionView.Search(sender,e);
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
                        }
                        
                    }
                }
                else if (AreListsEqual(KeysPressed, ConfigLoader.ConfigLoader.Settings.Shortcuts["newWindow"]))
                {
                    switch (RemoteView.Client.Type)
                    {
                        case ClientType.FTP:
                            var castFTP = (ClientTransferProtocol) RemoteView.Client;
                            new MainWindowRemote(
                                new ClientLocal(LocalView.Client.CurrentFolder!.Path), 
                                new ClientTransferProtocol(castFTP.Host, castFTP.Username, castFTP.Password)).Show();
                            break;
                        case ClientType.ONEDRIVE :
                            new MainWindowRemote(
                                new ClientLocal(LocalView.Client.CurrentFolder!.Path), 
                                new ClientOneDrive()).Show();
                            break;
                        case ClientType.GOOGLEDRIVE:
                            new MainWindowRemote(
                                new ClientLocal(LocalView.Client.CurrentFolder.Path), 
                                new ClientGoogleDrive()).Show();
                            break;
                    }
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["rename"]))
                    LocalView.ActionView.Rename(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["reload"]))
                    LocalView.Refresh();
                else if (KeysPressed ==ConfigLoader.ConfigLoader.Settings.Shortcuts["settings"])
                    new SettingsWindow(this).Show();
            }
        }
        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e) => KeysPressed.Remove(e.Key);
        
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


        private void Closed(object? sender, CancelEventArgs e)
        {
            IsClosed = true;
        }
    }
}