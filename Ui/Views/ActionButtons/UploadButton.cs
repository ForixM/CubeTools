using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using Library;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class UploadButton : ActionButton
{
    public UploadButton(ClientUI main, int def) : base(main, def)
    {
        _icon.Source = ResourcesIconsCompressed.UploadCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Upload");
    }

    private void OnClick(object sender)
    {
        Thread uploadThread = new Thread(() =>
        {
            Pointer temp = _main.ActionView.SelectedXaml[0].Pointer;
            List<Pointer> pointers = new List<Pointer>();
            foreach (PointerItem item in _main.ActionView.SelectedXaml)
            {
                pointers.Add(Pointer.DeepCopy(item.Pointer));
            }
            foreach (Pointer item in pointers)
            {
                if (item.IsDir)
                {
                    ((MainWindowRemote) _main.Main).RemoteView.Client.UploadFolder(_main.Client, item, ((MainWindowRemote)_main.Main).RemoteView.Client.CurrentFolder);
                }
                else
                {
                    ((MainWindowRemote) _main.Main).RemoteView.Client.UploadFile(_main.Client, item, ((MainWindowRemote)_main.Main).RemoteView.Client.CurrentFolder);
                }
            }
            Dispatcher.UIThread.Post(() => ((MainWindowRemote) _main.Main).RemoteView.Refresh());
        });
        uploadThread.Start();
    }
}