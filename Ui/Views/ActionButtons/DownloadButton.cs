using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using Library;
using Library.LibraryOneDrive;
using Library.ManagerExceptions;
using ResourcesLoader;
using Ui.Views.Error;

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
        if (_main.ActionView.SelectedXaml.Count == 0) return;
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
                try
                {
                    if (item.IsDir)
                    {
                        _main.Client.DownloadFolder(_main.Client, item,
                            ((MainWindowRemote) _main.Main).LocalView.Client.CurrentFolder);
                    }
                    else
                    {
                        _main.Client.DownloadFile(_main.Client, item,
                            ((MainWindowRemote) _main.Main).LocalView.Client.CurrentFolder);
                    }
                }
                catch (Exception)
                {
                    new ErrorBase(new ManagerException("Download error", Level.Normal, "Download error",
                        $"Could not download this item: {item.Name}")).Show();
                }
                Dispatcher.UIThread.Post(() => ((MainWindowRemote) _main.Main).LocalView.Refresh());
            }
        });
        downloadThread.Start();
    }
}