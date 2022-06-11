using System.Diagnostics;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class SortButton : ActionButton
{
    public SortButton(int def) : base(def)
    {
        _icon.Source = ResourcesIconsCompressed.SortCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        new Sort(_main).Show();
    }
}