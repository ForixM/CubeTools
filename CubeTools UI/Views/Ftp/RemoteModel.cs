using System.Collections.Generic;
using Library.Pointers;
using LibraryFTP;

namespace CubeTools_UI.Views.Ftp;

public class RemoteModel
{
    public FtpArboresence Remote;
    public List<IFtpItem> Children => Remote.Items;
    //public string RemotePath => Remote;
}