using System.Diagnostics;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class SortButton : ActionButton
{
    public SortButton(ClientUI main, int def) : base(main, def)
    {
        _icon.Source = ResourcesIconsCompressed.SortCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        new Sort(_main).Show();
    }
}