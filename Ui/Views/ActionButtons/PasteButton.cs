using System;
using System.Linq;
using Avalonia.Threading;
using Library;
using Library.ManagerExceptions;
using ResourcesLoader;
using Ui.Views.Actions;
using Ui.Views.Error;

namespace Ui.Views.ActionButtons;

public class PasteButton : ActionButton
{
    public PasteButton()
    {
        _icon.Source = ResourcesIconsCompressed.PasteCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        // 1) Copy Copied
        foreach (var item in _main.ActionView.CopiedXaml)
            CopyPointer(item.Pointer);

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
    
    private void CopyPointer(Pointer source)
    {
        try
        {
            // Copy Pointer
            Dispatcher.UIThread.Post(
                () =>
                {
                    _main.Client.Copy(source);
                },
                DispatcherPriority.MaxValue);
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