namespace LibraryFTP;

public class FtpFile : IFtpItem
{
    // private string _name;
    private int _size;
    // private string _parentPath;

    // public string Name => _name;
    public int Size => _size;
    // public string ParentPath => _parentPath;
    // public string Path => _parentPath + _name;
    
    public FtpFile(string name, int size, string parentPath)
    {
        this._name = name;
        this._size = size;
        this._parentPath = parentPath;
    }

    public override string ToString()
    {
        return $"{{name={_name}, size={_size}, path={Path}}}";
    }
}