using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using ResourcesLoader;
using Ui.Views.Error;

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
        if (_main.ActionView.SelectedXaml.Count == 0) return;
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
                try
                {
                    if (item.IsDir)
                    {
                        ((MainWindowRemote) _main.Main).RemoteView.Client.UploadFolder(_main.Client, item,
                            ((MainWindowRemote) _main.Main).RemoteView.Client.CurrentFolder);
                    }
                    else
                    {
                        ((MainWindowRemote) _main.Main).RemoteView.Client.UploadFile(_main.Client, item,
                            ((MainWindowRemote) _main.Main).RemoteView.Client.CurrentFolder);
                    }
                }
                catch (Exception)
                {
                    new ErrorBase(new ManagerException("Upload error", Level.Normal, "Upload error",
                        $"Could not upload this item: {item.Name}")).Show();
                }
            }
            Dispatcher.UIThread.Post(() => ((MainWindowRemote) _main.Main).RemoteView.Refresh());
        });
        uploadThread.Start();
    }
}