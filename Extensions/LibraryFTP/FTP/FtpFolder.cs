namespace LibraryFTP;

public class FtpFolder : IFtpItem
{
    private string _name;
    private string _parentPath;

    public string Name => _name;
    public string ParentPath => _parentPath;
    public string Path => _parentPath + _name;
    
    public FtpFolder(string name, string parentPath)
    {
        this._name = name;
        this._parentPath = parentPath;
    }

    public override string ToString()
    {
        return $"{{name={_name}}}";
    }
}