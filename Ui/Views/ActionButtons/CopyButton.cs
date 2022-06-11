using Avalonia.Controls;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class CopyButton : ActionButton
{
    public CopyButton(int def) : base(def)
    {
        _icon.Source = ResourcesIconsCompressed.CopyCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Copy");
    }

    private void OnClick(object sender)
    {
        _main.ActionView.CopiedXaml.Clear();
        _main.ActionView.CutXaml.Clear();
        foreach (var item in _main.ActionView.SelectedXaml) _main.ActionView.CopiedXaml.Add(item);
    }
}