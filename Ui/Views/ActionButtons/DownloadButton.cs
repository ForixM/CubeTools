using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using Library;
using Library.LibraryOneDrive;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class DownloadButton : ActionButton
{
    public DownloadButton(ClientUI main, int def) : base(main, def)
    {
        _icon.Source = ResourcesIconsCompressed.DownloadCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Download");
    }

    private void OnClick(object sender)
    {
        Thread downloadThread = new Thread(() =>
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
                    _main.Client.DownloadFolder(_main.Client, item, ((MainWindowRemote)_main.Main).LocalView.Client.CurrentFolder);
                }
                else
                {
                    _main.Client.DownloadFile(_main.Client, item, ((MainWindowRemote)_main.Main).LocalView.Client.CurrentFolder);
                }
                Dispatcher.UIThread.Post(() => ((MainWindowRemote) _main.Main).LocalView.Refresh());
            }
        });
        downloadThread.Start();
    }
}