using LibraryCompression;

namespace InitLoader
{
    /// <summary>
    /// This class will help to initialize directories and path needed for the application so that it can run correctly
    /// </summary>
    public static partial class InitLoader
    {
        private static void InitLibraries() => Compression.Init();
    }
}