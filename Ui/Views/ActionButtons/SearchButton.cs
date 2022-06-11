using System.Diagnostics;
using Avalonia.Layout;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class SearchButton : ActionButton
{
    public SearchButton(int def) : base(def)
    {
        _icon.Source = ResourcesIconsCompressed.SearchCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        new Search(_main).Show();
    }
}