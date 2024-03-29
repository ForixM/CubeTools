using Library.DirectoryPointer;
using Library.FilePointer;

namespace Library {

    public class ClientLocal : Client
    {

        public ClientLocal() : base(ClientType.LOCAL)
        {
            string path = Directory.GetCurrentDirectory().Replace('\\', '/');
            Root = new DirectoryLocalPointer(Directory.GetDirectoryRoot(path));
            CurrentFolder = new DirectoryLocalPointer(path);
            Children = ListChildren();
        }

        public ClientLocal(string path) : base(ClientType.LOCAL)
        {
            Root = new DirectoryLocalPointer(Directory.GetDirectoryRoot(path));
            CurrentFolder = new DirectoryLocalPointer(path);
            Children = ListChildren();
        }

        #region Actions

        public override Pointer CreateFile(string name) => ManagerWriter.ManagerWriter.Create(CurrentFolder.Path+"/"+name);

        public override Pointer CreateFolder(string name) => ManagerWriter.ManagerWriter.CreateDir(CurrentFolder.Path + "/" + name);

        public override Pointer? Copy(Pointer pointer, Pointer destination) => ManagerWriter.ManagerWriter.Copy(pointer.Path, destination.Path);

        public override void Delete(Pointer pointer) =>  ManagerWriter.ManagerWriter.Delete(pointer.Path, true);

        public override void Rename(Pointer pointer, string newName) =>  ManagerWriter.ManagerWriter.Rename(pointer.Path, CurrentFolder.Path + "/" + newName);

        public override void UploadFile(Client source, Pointer pointer, Pointer destination)
        {
            if (pointer is LocalPointer)
            {
                source.UploadFile(source, pointer, destination);
            }
        }

        public override void UploadFolder(Client source, Pointer localPointer, Pointer destination)
        {
            if (localPointer is LocalPointer)
            {
                source.UploadFolder(source, localPointer, destination);
            }
        }

        public override void AccessPath(Pointer destination)
        {
            CurrentFolder = new DirectoryLocalPointer(destination.Path);
            Directory.SetCurrentDirectory(CurrentFolder.Path);
            foreach (var pointer in Children) pointer.Dispose();
            GC.Collect();
            Children.Clear();
            Children = ListChildren();
        }

        #endregion

        #region Properties

        public override Pointer? GetItem(string path, bool isAbsolute = false)
        {
            if (!isAbsolute)  path = CurrentFolder!.Path + "/" + path;
            if (Directory.Exists(path))
                return new DirectoryLocalPointer(path);
            if (File.Exists(path))
                return new FileLocalPointer(path);
            return null;
        }

        public override string GetItemName(Pointer pointer) => ManagerReader.ManagerReader.GetPathToName(pointer.Path);

        public override string GetItemType(Pointer pointer) => ManagerReader.ManagerReader.GetFileExtension(pointer.Path);

        public override long GetItemSize(Pointer pointer) => ((LocalPointer) pointer).GetPointerSize();

        public override Pointer GetParentReference(Pointer pointer) =>  Root.Path == pointer.Path ? pointer : new DirectoryLocalPointer(pointer.ParentPath);

        public override List<Pointer>? ListChildren() => ((DirectoryLocalPointer) CurrentFolder).GetChildren().Cast<Pointer>().ToList();

        public override void InitializeProperties(Pointer pointer)
        {
            pointer.Name = GetItemName(pointer);
            pointer.Size = GetItemSize(pointer);
            pointer.Type = GetItemType(pointer);
            pointer.ParentPath = GetParentReference(pointer).Path;
        }

        public override string GetItemAccessDate(Pointer pointer) => ((LocalPointer) pointer).GetPointerAccessDate();

        public override string GetItemCreationDate(Pointer pointer) => ((LocalPointer) pointer).GetPointerCreationDate();

        public override string GetItemLastEditionDate(Pointer pointer) => ((LocalPointer) pointer).GetPointerLastEdition();

        public override bool GetItemHiddenProperty(Pointer pointer) => ((LocalPointer) pointer).IsHidden();

        public override bool GetItemReadOnlyProperty(Pointer pointer) => ((LocalPointer) pointer).IsReadOnly();

        public override void SetHiddenProperty(Pointer pointer, bool set) => ManagerWriter.ManagerWriter.SetAttributes((LocalPointer) pointer, set, FileAttributes.Hidden);

        public override void SetReadOnlyProperty(Pointer pointer, bool set) => ManagerWriter.ManagerWriter.SetAttributes((LocalPointer) pointer, set, FileAttributes.ReadOnly);

        #endregion
    }
}