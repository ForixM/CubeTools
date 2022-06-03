using System.Runtime.Serialization;
using Avalonia.Input;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class ShortcutsSettings
    {
        public List<List<Key>> Shortcuts =>
            new()
            {
                CopyShortCut, CutShortCut, CloseShortCut, PasteShortCut, NewWindowShortCut, SearchShortCut,
                DeleteShortCut, DeletePermanentShortCut, SelectAllShortcut, CreateFileShortcut, CreateDirShortcut,
                RenameShortcut, SettingsShortcut, ReloadShortcut
            };
        
        public Dictionary<string, List<Key>> ShortCutsDictionnary =>
            new()
            {
                {"Copy",CopyShortCut}, {"Cut",CutShortCut}, {"Close",CloseShortCut}, {"Paste",PasteShortCut}, {"NewWindow",NewWindowShortCut}, 
                {"Search",SearchShortCut}, {"Delete",DeleteShortCut}, {"DeletePermanent",DeletePermanentShortCut}, {"SelectAll",SelectAllShortcut}, 
                {"CreateFile",CreateFileShortcut}, {"CreateDir",CreateDirShortcut}, {"Rename",RenameShortcut}, {"Settings",SettingsShortcut}
                ,{"Reload",ReloadShortcut}
            };

        public List<Key> CopyShortCut
        {
            get => copyShortcut != null ? FromStringToKeys(copyShortcut) : new List<Key>();
            set => copyShortcut = FromKeysToString(value);
        }
        public List<Key> CutShortCut
        {
            get => cutShortcut != null ? FromStringToKeys(cutShortcut) : new List<Key>();
            set => cutShortcut = FromKeysToString(value);
        }
        public List<Key> CloseShortCut
        {
            get => closeShortcut != null ? FromStringToKeys(closeShortcut) : new List<Key>();
            set => closeShortcut = FromKeysToString(value);
        }
        public List<Key> PasteShortCut
        {
            get => pasteShortcut != null ? FromStringToKeys(pasteShortcut) : new List<Key>();
            set => pasteShortcut = FromKeysToString(value);
        }
        public List<Key> NewWindowShortCut
        {
            get => newWindowShortcut != null ? FromStringToKeys(newWindowShortcut) : new List<Key>();
            set => newWindowShortcut = FromKeysToString(value);
        }
        public List<Key> SearchShortCut
        {
            get => searchShortcut != null ? FromStringToKeys(searchShortcut) : new List<Key>();
            set => searchShortcut = FromKeysToString(value);
        }
        public List<Key> DeleteShortCut
        {
            get => deleteShortcut != null ? FromStringToKeys(deleteShortcut) : new List<Key>();
            set => deleteShortcut = FromKeysToString(value);
        }
        public List<Key> DeletePermanentShortCut
        {
            get => deletePermanentShortcut != null ? FromStringToKeys(deletePermanentShortcut) : new List<Key>();
            set => deletePermanentShortcut = FromKeysToString(value);
        }
        public List<Key> SelectAllShortcut
        {
            get => selectAllShortcut != null ? FromStringToKeys(selectAllShortcut) : new List<Key>();
            set => selectAllShortcut = FromKeysToString(value);
        }

        public List<Key> CreateFileShortcut
        {
            get => createFileShortcut != null ? FromStringToKeys(createFileShortcut) : new List<Key>();
            set => createFileShortcut = FromKeysToString(value);
        }
        public List<Key> CreateDirShortcut
        {
            get => createDirShortcut != null ? FromStringToKeys(createDirShortcut) : new List<Key>();
            set => createDirShortcut = FromKeysToString(value);
        }

        public List<Key> RenameShortcut
        {
            get => renameShortcut != null ? FromStringToKeys(renameShortcut) : new List<Key>();
            set => renameShortcut = FromKeysToString(value);
        }
        public List<Key> SettingsShortcut
        {
            get => settingsShortcut != null ? FromStringToKeys(settingsShortcut) : new List<Key>();
            set => settingsShortcut = FromKeysToString(value);
        }

        public List<Key> ReloadShortcut
        {
            get => reloadShortcut != null ? FromStringToKeys(reloadShortcut) : new List<Key>();
            set => reloadShortcut = FromKeysToString(value);
        }

        [DataMember(Name = "copy")] private string? copyShortcut { get; set; }
        [DataMember(Name = "cut")] private string? cutShortcut { get; set; }
        [DataMember(Name="close")] private string? closeShortcut { get; set; }
        [DataMember(Name="paste")] private string? pasteShortcut { get; set; }
        [DataMember(Name="newWindow")] private string? newWindowShortcut { get; set; }
        [DataMember(Name="delete")] private string? deleteShortcut { get; set; }
        [DataMember(Name = "search")] private string? searchShortcut { get; set; }
        [DataMember(Name= "deletePermanent")] private string? deletePermanentShortcut { get; set; }
        [DataMember(Name="selectAll")] private string? selectAllShortcut { get; set; }
        [DataMember(Name="createFile")] private string? createFileShortcut { get; set; }
        [DataMember(Name="createDir")] private string? createDirShortcut { get; set; }
        [DataMember(Name="rename")] private string? renameShortcut { get; set; }
        [DataMember(Name="settings")] private string? settingsShortcut { get; set; }
        [DataMember(Name="reload")] private string? reloadShortcut { get; set; }

        #region Converter
        private static List<Key> FromStringToKeys(string s)
        {
            var keys = new List<Key>();
            foreach (var key in s.Split('+'))
            {
                switch (key)
                {
                    case "A":
                        keys.Add(Key.A);
                        break;
                    case "B":
                        keys.Add(Key.B);
                        break;
                    case "C":
                        keys.Add(Key.C);
                        break;
                    case "D":
                        keys.Add(Key.D);
                        break;
                    case "E":
                        keys.Add(Key.E);
                        break;
                    case "F":
                        keys.Add(Key.F);
                        break;
                    case "G":
                        keys.Add(Key.G);
                        break;
                    case "H":
                        keys.Add(Key.H);
                        break;
                    case "I":
                        keys.Add(Key.I);
                        break;
                    case "J":
                        keys.Add(Key.J);
                        break;
                    case "K":
                        keys.Add(Key.K);
                        break;
                    case "L":
                        keys.Add(Key.L);
                        break;
                    case "M":
                        keys.Add(Key.M);
                        break;
                    case "N":
                        keys.Add(Key.N);
                        break;
                    case "O":
                        keys.Add(Key.O);
                        break;
                    case "P":
                        keys.Add(Key.P);
                        break;
                    case "Q":
                        keys.Add(Key.Q);
                        break;
                    case "R":
                        keys.Add(Key.R);
                        break;
                    case "S":
                        keys.Add(Key.S);
                        break;
                    case "T":
                        keys.Add(Key.T);
                        break;
                    case "U":
                        keys.Add(Key.U);
                        break;
                    case "V":
                        keys.Add(Key.V);
                        break;
                    case "W":
                        keys.Add(Key.W);
                        break;
                    case "X":
                        keys.Add(Key.X);
                        break;
                    case "Y":
                        keys.Add(Key.Y);
                        break;
                    case "Z":
                        keys.Add(Key.Z);
                        break;
                    case "ENTER":
                        keys.Add(Key.Enter);
                        break;
                    case "SPACE":
                        keys.Add(Key.Space);
                        break;
                    case "SHIFT":
                        keys.Add(Key.LeftShift);
                        break;
                    case "CTRL":
                        keys.Add(Key.LeftCtrl);
                        break;
                    case "CANCEL":
                        keys.Add(Key.Cancel);
                        break;
                    case "DELETE":
                        keys.Add(Key.Delete);
                        break;
                    case "INSERT":
                        keys.Add(Key.Insert);
                        break;
                    case "F1":
                        keys.Add(Key.F1);
                        break;
                    case "F2":
                        keys.Add(Key.F2);
                        break;
                    case "F3":
                        keys.Add(Key.F3);
                        break;
                    case "F4":
                        keys.Add(Key.F4);
                        break;
                    case "F5":
                        keys.Add(Key.F5);
                        break;
                    case "F6":
                        keys.Add(Key.F6);
                        break;
                    case "F7":
                        keys.Add(Key.F7);
                        break;
                    case "F8":
                        keys.Add(Key.F8);
                        break;
                    case "F9":
                        keys.Add(Key.F9);
                        break;
                    case "F10":
                        keys.Add(Key.F10);
                        break;
                    case "F11":
                        keys.Add(Key.F11);
                        break;
                    case "F12":
                        keys.Add(Key.F12);
                        break;
                    case "TAB":
                        keys.Add(Key.Tab);
                        break;
                    case "ALT":
                        keys.Add(Key.LeftAlt);
                        break;
                    case "FN" :
                        keys.Add(Key.FnLeftArrow);
                        break;
                    default :
                        keys.Add(Key.None);
                        break;
                }
            }
            return keys;
        }
        public string FromKeysToString(List<Key> keys)
        {
            string s = "";
            foreach (var key in keys)
            {
                switch (key)
                {
                    case Key.A:
                        s += "A";
                        break;
                    case Key.B:
                        s += "B";
                        break;
                    case Key.C:
                        s += "C";
                        break;
                    case Key.D:
                        s += "D";
                        break;
                    case Key.E:
                        s += "E";
                        break;
                    case Key.F:
                        s += "F";
                        break;
                    case Key.G:
                        s += "G";
                        break;
                    case Key.H:
                        s += "H";
                        break;
                    case Key.I:
                        s += "I";
                        break;
                    case Key.J:
                        s += "J";
                        break;
                    case Key.K:
                        s += "K";
                        break;
                    case Key.L:
                        s += "L";
                        break;
                    case Key.M:
                        s += "M";
                        break;
                    case Key.N:
                        s += "N";
                        break;
                    case Key.O:
                        s += "O";
                        break;
                    case Key.P:
                        s += "P";
                        break;
                    case Key.Q:
                        s += "Q";
                        break;
                    case Key.R:
                        s += "R";
                        break;
                    case Key.S:
                        s += "S";
                        break;
                    case Key.T:
                        s += "T";
                        break;
                    case Key.U:
                        s += "U";
                        break;
                    case Key.V:
                        s += "V";
                        break;
                    case Key.W:
                        s += "W";
                        break;
                    case Key.X:
                        s += "X";
                        break;
                    case Key.Y:
                        s += "Y";
                        break;
                    case Key.Z:
                        s += "Z";
                        break;
                    case Key.Enter:
                        s += "ENTER";
                        break;
                    case Key.Space:
                        s += "SPACE";
                        break;
                    case Key.LeftShift:
                        s += "SHIFT";
                        break;
                    case Key.LeftCtrl:
                        s += "CTRL";;
                        break;
                    case Key.Cancel:
                        s += "CANCEL";
                        break;
                    case Key.Delete:
                        s += "DELETE";
                        break;
                    case Key.Insert:
                        s += "INSERT";
                        break;
                    case Key.F1:
                        s += "F1";
                        break;
                    case Key.F2:
                        s += "F2";
                        break;
                    case Key.F3:
                        s += "F3";
                        break;
                    case Key.F4:
                        s += "F4";
                        break;
                    case Key.F5:
                        s += "F5";
                        break;
                    case Key.F6:
                        s += "F6";
                        break;
                    case Key.F7:
                        s += "F7";
                        break;
                    case Key.F8:
                        s += "F8";
                        break;
                    case Key.F9:
                        s += "F9";
                        break;
                    case Key.F10:
                        s += "F10";
                        break;
                    case Key.F11:
                        s += "F11";
                        break;
                    case Key.F12:
                        s += "F12";
                        break;
                    case Key.Tab:
                        s += "TAB";
                        break;
                    case Key.LeftAlt:
                        s += "ALT";
                        break;
                    case Key.FnLeftArrow:
                        s += "FN";
                        break;
                }

                if (key != Key.None)
                    s += "+";
            }
            return s;
        }
        
        #endregion

    }
}