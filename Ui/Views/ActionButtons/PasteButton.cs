using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using Avalonia.Controls;
using Avalonia.Threading;
using Library.ManagerExceptions;
using ResourcesLoader;
using Ui.Views.Actions;
using Ui.Views.Error;
using Pointer = Library.Pointer;

namespace Ui.Views.ActionButtons;

public class PasteButton : ActionButton
{
    public PasteButton(int def) : base(def)
    {
        _icon.Source = ResourcesIconsCompressed.PasteCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Paste");
    }

    private void OnClick(object sender)
    {
        // 1) Copy Copied
        Pointer path = Pointer.DeepCopy(_main.Client.CurrentFolder);
        Thread copyThread = new Thread(() =>
        {
            foreach (var item in _main.ActionView.CopiedXaml)
                CopyPointer(item.Pointer, path);
        });
        copyThread.Start();

        switch (_main.ActionView.CutXaml.Count)
        {
            case 0:
                return;
            case 1:
                new Delete(_main, _main.ActionView.CutXaml[0].Pointer).Show();
                break;
            default:
                new DeleteMultiple(_main, _main.ActionView.CutXaml.Select(pointer => pointer.Pointer).ToList()).Show();
                break;
        }
    }
    
    private void CopyPointer(Pointer source, Pointer destination)
    {
        try
        {
            // Copy Pointer
            _main.Client.Copy(source, destination);
            Dispatcher.UIThread.Post(_main.Refresh);
            // Dispatcher.UIThread.Post(
            //     () =>
            //     {
            //         _main.Client.Copy(source);
            //         _main.Refresh();
            //     },
            //     DispatcherPriority.MaxValue);
        }
        catch (Exception exception)
        {
            if (exception is ManagerException @managerException)
            {
                @managerException.Errorstd = $"Unable to copy {source.Name}";
                new ErrorBase(@managerException).ShowDialog<object>(_main.Main);
            }
        }
    }
}