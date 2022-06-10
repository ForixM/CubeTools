using System.Diagnostics;
using Avalonia.Controls;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class UploadButton : ActionButton
{
    public UploadButton()
    {
        _icon.Source = ResourcesIconsCompressed.UploadCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Upload");
    }

    private void OnClick(object sender)
    {
        foreach (PointerItem item in _main.ActionView.SelectedXaml)
        {
            if (item.Pointer.IsDir)
            {
                ((MainWindowRemote) _main.Main).RemoteView.Client.UploadFolder(_main.Client, item.Pointer, ((MainWindowRemote)_main.Main).RemoteView.Client.CurrentFolder);
            }
            else
            {
                ((MainWindowRemote) _main.Main).RemoteView.Client.UploadFile(_main.Client, item.Pointer, ((MainWindowRemote)_main.Main).RemoteView.Client.CurrentFolder);
            }
        }
        ((MainWindowRemote) _main.Main).RemoteView.Refresh();
        Debug.Print("upload");
    }
}