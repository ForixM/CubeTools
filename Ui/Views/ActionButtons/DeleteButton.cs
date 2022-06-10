using System.Linq;
using Avalonia.Controls;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class DeleteButton : ActionButton
{
    public DeleteButton()
    {
        _icon.Source = ResourcesIconsCompressed.DeleteCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Delete");
    }

    private void OnClick(object sender)
    {
        switch (_main.ActionView.SelectedXaml.Count)
        {
            case 0:
                return;
            case 1:
                new Delete(_main, _main.ActionView.SelectedXaml[0].Pointer).Show();
                break;
            default:
                new DeleteMultiple(_main, _main.ActionView.SelectedXaml.Select(pointer => pointer.Pointer).ToList())
                    .Show();
                break;
        }

        
    }
}