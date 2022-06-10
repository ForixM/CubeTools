using Avalonia.Controls;
using Library.ManagerExceptions;
using ResourcesLoader;
using Ui.Views.Actions;
using Ui.Views.Error;

namespace Ui.Views.ActionButtons;

public class RenameButton : ActionButton
{
    public RenameButton()
    {
        _icon.Source = ResourcesIconsCompressed.RenameCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Rename");
    }
    
    private void OnClick(object? sender)
    {
        switch (_main.ActionView.SelectedXaml.Count)
        {
            case < 1:
                return;
            case 1:
                new Rename(_main.ActionView.SelectedXaml[0].Pointer, _main.Client.Children, _main).Show();
                break;
            default:
                new ErrorBase(new ManagerException("Unable to rename multiple data")).ShowDialog<object>(_main.Main);
                break;
        }
    }
}