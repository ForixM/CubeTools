using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Ui.Views.Local;
using Ui.Views.Settings;

namespace Ui.Views.MainWindow
{
    public class MainWindow : Window
    {
        
        public List<Key> KeysPressed;
        public LinkBar LinkBarView;
        public Local.Local LocalView;
        public static MainWindow LastReference;
        
        public MainWindow()
        {
            LastReference = this;
            InitializeComponent();
            LinkBarView = this.FindControl<LinkBar>("LinkBar");
            LocalView = this.FindControl<Local.Local>("Local");
            LinkBarView.Main = LocalView;
            LinkBarView.InitializeExpanders();
            KeysPressed = new List<Key>();
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #region Events
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            
            if (KeysPressed.Contains(e.Key)) return;
            KeysPressed.Add(e.Key);
            if (IsListInListList(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.Shortcuts))
            {
                if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CloseShortCut))
                    Close();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CreateDirShortcut))
                    LocalView.ActionBarView.CreatDir(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CreateFileShortcut))
                    LocalView.ActionBarView.CreateFile(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CutShortCut))
                    LocalView.ActionBarView.Cut(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CopyShortCut))
                    LocalView.ActionBarView.Copy(sender, e);
                else if (KeysPressed == ConfigLoader.ConfigLoader.Settings.Shortcuts.DeleteShortCut)
                    LocalView.ActionBarView.Delete(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.PasteShortCut))
                    LocalView.ActionBarView.Paste(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.SearchShortCut))
                    LocalView.ActionBarView.Search(sender,e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.SelectAllShortcut))
                {
                    // All items are selected
                    if (LocalView.ActionBarView.SelectedXaml.Count == LocalView.PathsBarView.Generator.Children.Count)
                    {
                        LocalView.ActionBarView.SelectedXaml.Clear();
                        int size = LocalView.PathsBarView.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            LocalView.ActionBarView.SelectedXaml.Add((PointerItem) LocalView.PathsBarView.Generator.Children[i]);
                            size = LocalView.PathsBarView.Generator.Children.Count;
                        }
                    }
                    else
                    {
                        int size = LocalView.PathsBarView.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            if (!LocalView.ActionBarView.SelectedXaml.Contains((PointerItem) LocalView.PathsBarView.Generator.Children[i]))
                                LocalView.ActionBarView.SelectedXaml.Add((PointerItem) LocalView.PathsBarView.Generator.Children[i]);
                            size = LocalView.PathsBarView.Generator.Children.Count;
                        }
                        
                    }
                }
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.NewWindowShortCut))
                    new MainWindow().Show();
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.RenameShortcut))
                    LocalView.ActionBarView.Rename(sender, e);
                else if (AreListsEqual(KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.ReloadShortcut))
                    LocalView.Refresh();
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