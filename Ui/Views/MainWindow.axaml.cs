using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Cryptography;
using Avalonia.Controls;
using Avalonia.Controls.Primitives.PopupPositioning;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Library;
using Ui.Views.ActionButtons;
using Ui.Views.Actions;
using Ui.Views.MenuController;
using Ui.Views.Settings;
using Menu = Ui.Views.MenuController.Menu;

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
                new CreateFileButton(0), new CreateFolderButton(1), new CopyButton(2), new CutButton(3),
                new PasteButton(4),
                new RenameButton(5), new CompressButton(6), new DeleteButton(7) , new SortButton(9),
                new SearchButton(10), new SnapdropButton(11), new SmashButton(12)
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
            if (LocalView.subGrid.Children[0] is Menu) return;
            KeysPressed.Add(e.Key);
            if (IsListInListList(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts))
            {
                if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["close"]))
                    Close();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["createDir"]))
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
                    LocalView.ActionView.Delete(sender, e);
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["paste"]))
                {
                    LocalView.ActionView.Paste(sender, e);
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
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts["newWindow"]))
                {
                    new MainWindow().Show();
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
                else if (KeysPressed ==ConfigLoader.ConfigLoader.Settings.Shortcuts["settings"])
                {
                    new SettingsWindow().Show();
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