using System.Collections.Generic;
using LibraryFTP;

namespace CubeTools_UI.Models;

public class FTPNavigationBarModel
{
    private int _index;
    private List<FtpFolder> _queue;
    
    public int Index
    {
        get => _index;
        set => _index = value;
    }
    public List<FtpFolder> Queue => _queue;

    public void Add(FtpFolder folder)
    {
        if (_queue.Count-1 == Index || Index < 0)
        {
            _queue.Add(folder);
        } else if (_queue.Count > Index+1 && folder != _queue[Index + 1])
        {
            _queue.RemoveRange(Index+1, _queue.Count-Index-1);
            _queue.Add(folder);
        }
        Index++;
    }

    public FTPNavigationBarModel()
    {
        _index = -1;
        _queue = new List<FtpFolder>();
    }
}