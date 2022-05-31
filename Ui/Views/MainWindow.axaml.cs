using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Ui.Models;
using Ui.Views.Actions;
using Ui.Views.Settings;

namespace Ui.Views
{
    public class MainWindow : Window
    {

        public MainWindowModel Model;
        
        public MainWindow() : base()
        {
            InitializeComponent();
            Model = new MainWindowModel(this);
        }

        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        #region Events
        
        private void OnKeyPressedWindow(object? sender, KeyEventArgs e)
        {
            
            if (Model.KeysPressed.Contains(e.Key)) return;
            Model.KeysPressed.Add(e.Key);
            if (IsListInListList(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.Shortcuts))
            {
                if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CloseShortCut))
                    Close();
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CopyShortCut))
                    Model.ModelLocal.ModelActionBar.View.Copy(sender, e);
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CreateDirShortcut))
                    new CreateFolderPopUp().Show();
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CreateFileShortcut))
                    new CreatePopUp().Show();
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CutShortCut))
                    Model.ModelLocal.ModelActionBar.View.Cut(sender, e);
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.CopyShortCut))
                    Model.ModelLocal.ModelActionBar.View.Copy(sender, e);
                else if (Model.KeysPressed ==ConfigLoader.ConfigLoader.Settings.Shortcuts.DeleteShortCut)
                    Model.ModelLocal.ModelActionBar.View.Delete(sender, e);
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.PasteShortCut))
                    Model.ModelLocal.ModelActionBar.View.Paste(sender, e);
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.SearchShortCut))
                    Model.ModelLocal.ModelActionBar.View.Search(sender,e);
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.SelectAllShortcut))
                {
                    // All items are selected
                    if (Model.ModelLocal.ModelActionBar.SelectedXaml.Count == Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count)
                    {
                        
                        Model.ModelLocal.ModelActionBar.SelectedXaml.Clear();
                        int size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            Model.ModelLocal.ModelActionBar.SelectedXaml.Add((PointerItem) Model.ModelLocal.ModelPathsBar.View.Generator.Children[i]);
                            size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                        }
                        //Model.ModelLocal.ModelActionBar.View.
                    }
                    else
                    {
                        int size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                        for (int i = 0; i < size; i++)
                        {
                            if (!Model.ModelLocal.ModelActionBar.SelectedXaml.Contains((PointerItem)Model.ModelLocal.ModelPathsBar.View.Generator.Children[i]))
                                Model.ModelLocal.ModelActionBar.SelectedXaml.Add((PointerItem) Model.ModelLocal.ModelPathsBar.View.Generator.Children[i]);
                            size = Model.ModelLocal.ModelPathsBar.View.Generator.Children.Count;
                        }
                        
                    }
                }
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.NewWindowShortCut))
                    new MainWindow().Show();
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.RenameShortcut))
                    Model.ModelLocal.ModelActionBar.View.Rename(sender, e);
                else if (AreListsEqual(Model.KeysPressed,ConfigLoader.ConfigLoader.Settings.Shortcuts.ReloadShortcut))
                    Model.ModelLocal.ReloadPath();
                else if (Model.KeysPressed ==ConfigLoader.ConfigLoader.Settings.Shortcuts.SettingsShortcut)
                    new SettingsWindow().Show();
            }
        }
        private void OnKeyReleasedWindow(object? sender, KeyEventArgs e)
        {
            while (Model.KeysPressed.Count != 0 && Model.KeysPressed.Last() != e.Key)
                Model.KeysPressed.RemoveAt(Model.KeysPressed.Count - 1);
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