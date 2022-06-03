namespace Library.DirectoryPointer.DirectoryPointerLoaded
{
    public partial class DirectoryPointerLoaded : DirectoryPointer
    {
        // This region contains every functions that raise events when a modification occured of a pointer (DATE)

        /// <summary>
        ///     Raising event whenever a modification occured
        /// </summary>
        private void OnChanged(object sender, FileSystemEventArgs e) => ReloadPointer(e.FullPath);

        /// <summary>
        ///     Raising event when a creation
        /// </summary>
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            string created = e.FullPath.Replace("\\", "/");
            if (ManagerReader.ManagerReader.GetParent(created) != Path)
            {
                if (File.Exists(created)) _childrenFiles.Add(new FilePointer.FilePointer(created));
                else _childrenFiles.Add(new DirectoryPointer(created));
            }
        }

        /// <summary>
        ///     Raising event when deleting
        /// </summary>
        private void OnDeleted(object sender, FileSystemEventArgs e) => Remove(e.FullPath.Replace('\\','/'));

        /// <summary>
        ///     Raising event when renaming
        /// </summary>
        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            var save = GetChild(e.OldFullPath);
            Remove(e.OldFullPath);
            save.Path = e.FullPath.Replace('\\','/');
            ChildrenFiles.Add(save);
            ReloadPointer(save.Path);
        }

    }
}