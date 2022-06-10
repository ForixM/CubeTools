using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.MenuController;
using Ui.Views.Settings;

namespace Ui.Views
{
    public class MainWindow : Window
    {
        public bool IsClosed;
        public List<Key> KeysPressed;
        public LinkBar.LinkBar LinkBarView;
        public ClientUI LocalView;
        public static MainWindow LastReference;
        
        public MainWindow()
        {
            LastReference = this;
            InitializeComponent();
            LinkBarView = this.FindControl<LinkBar.LinkBar>("LinkBar");
            LocalView = new ClientUI(new ClientLocal(), this);
            LocalView.ActionView.SetActionButtons(new List<ActionButton>
            {
                new CreateFileButton(), new CreateFolderButton(), new CopyButton(), new CutButton(), new PasteButton(),
                new RenameButton(), new DeleteButton()
            });
            this.FindControl<Grid>("ClientLocal").Children.Add(LocalView);
            // Grid grid = this.FindControl<Grid>("ClientLocal");
            // grid.Children.Add(new Ui.Views.MenuController.Menu());
            LinkBarView.Main = LocalView;
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
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["newWindow"]))
                    new MainWindow().Show();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["rename"]))
                    LocalView.ActionView.Rename(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["reload"]))
                    LocalView.Refresh();
                else if (KeysPressed ==ConfigLoader.ConfigLoader.Settings.Shortcuts["settings"])
                    new SettingsWindow().Show();
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