namespace Library.ManagerWriter
{
    public partial class ManagerWriter
    {
        // This region contains every async version of the CubeTools software

        #region Copy

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        public static async Task CopyAsync(string source, string dest) =>
            await Task.Run(() => Copy(source, dest, true));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointer"></param>
        /// <returns></returns>
        public static async Task<Pointer> CopyAsync(Pointer pointer) => await Task.Run(() => Copy(pointer.Path));

        #endregion

        #region Delete

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public static async Task DeleteAsync(string dest) => await Task.Run(() => DeleteFile(dest));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointer"></param>
        public static async void DeleteAsync(Pointer pointer)
        {
            await Task.Run(() =>
            {
                switch (pointer)
                {
                    case FilePointer.FilePointer filePointer:
                        DeleteFile(filePointer);
                        break;
                    case DirectoryPointer.DirectoryPointer directoryPointer:
                        DeleteDir(directoryPointer);
                        break;
                }
            });
        }
        #endregion
        
    }
}

