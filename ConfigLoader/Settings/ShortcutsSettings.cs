using System.Runtime.Serialization;

namespace ConfigLoader.Settings
{
    [DataContract]
    public class ShortcutsSettings
    {
        public List<string> CopyShortCut
        {
            get => copyShortcut!.Split('+').ToList();
            set
            {
                copyShortcut = "";
                foreach (var s in value)
                    copyShortcut += s;
            }
        }
        public List<string> CutShortCut
        {
            get => cutShortcut!.Split('+').ToList();
            set
            {
                cutShortcut = "";
                foreach (var s in value)
                    cutShortcut += s;
            }
        }
        public List<string> CloseShortCut
        {
            get => closeShortcut!.Split('+').ToList();
            set
            {
                closeShortcut = "";
                foreach (var s in value)
                    closeShortcut += s;
            }
        }
        public List<string> PasteShortCut
        {
            get => pasteShortcut!.Split('+').ToList();
            set
            {
                pasteShortcut = "";
                foreach (var s in value)
                    pasteShortcut += s;
            }
        }
        public List<string> NewWindowShortCut
        {
            get => newWindowShortcut!.Split('+').ToList();
            set
            {
                newWindowShortcut = "";
                foreach (var s in value)
                    newWindowShortcut += s;
            }
        }
        public List<string> SearchShortCut
        {
            get => searchShortcut!.Split('+').ToList();
            set
            {
                searchShortcut = "";
                foreach (var s in value)
                    searchShortcut += s;
            }
        }
        public List<string> DeleteShortCut
        {
            get => deleteShortcut!.Split('+').ToList();
            set
            {
                deleteShortcut = "";
                foreach (var s in value)
                    deleteShortcut += s;
            }
        }
        public List<string> DeletePermanentShortCut
        {
            get => deletePermanentShortcut!.Split('+').ToList();
            set
            {
                deletePermanentShortcut = "";
                foreach (var s in value)
                    deletePermanentShortcut += s;
            }
        }
        public List<string> SelectAllShortcut
        {
            get => selectAllShortcut!.Split('+').ToList();
            set
            {
                selectAllShortcut = "";
                foreach (var s in value)
                    selectAllShortcut += s;
            }
        }

        public List<string> CreateFileShortcut
        {
            get => createFileShortcut!.Split('+').ToList();
            set
            {
                createFileShortcut = "";
                foreach (var s in value)
                    createFileShortcut += s;
            }
        }
        public List<string> CreateDirShortcut
        {
            get => createDirShortcut!.Split('+').ToList();
            set
            {
                createDirShortcut = "";
                foreach (var s in value)
                    createDirShortcut += s;
            }
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

    }
}