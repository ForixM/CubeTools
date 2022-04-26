using System.Collections.Generic;
using Library.Pointers;

namespace CubeTools_UI.Views.Ftp;

public class LocalModel
{
    public DirectoryType Local;
    public List<FileType> Children => Local.ChildrenFiles;
    public string LocalPath => Local.Path;
}