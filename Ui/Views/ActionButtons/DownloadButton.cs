using System.Diagnostics;
using Avalonia.Controls;
using Library;
using Library.LibraryOneDrive;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class DownloadButton : ActionButton
{
    public DownloadButton()
    {
        _icon.Source = ResourcesIconsCompressed.DownloadCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Download");
    }

    private void OnClick(object sender)
    {
        foreach (PointerItem item in _main.ActionView.SelectedXaml)
        {
            if (item.Pointer.IsDir)
            {
                _main.Client.DownloadFolder(_main.Client, item.Pointer, ((MainWindowRemote)_main.Main).LocalView.Client.CurrentFolder);
            }
            else
            {
                _main.Client.DownloadFile(_main.Client, item.Pointer, ((MainWindowRemote)_main.Main).LocalView.Client.CurrentFolder);
            }

        }
        ((MainWindowRemote) _main.Main).LocalView.Refresh();
        Debug.Print("download");
    }
}