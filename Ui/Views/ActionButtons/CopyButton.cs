using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class CopyButton : ActionButton
{
    public CopyButton()
    {
        _icon.Source = ResourcesIconsCompressed.CopyCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        _main.ActionView.CopiedXaml.Clear();
        _main.ActionView.CutXaml.Clear();
        foreach (var item in _main.ActionView.SelectedXaml) _main.ActionView.CopiedXaml.Add(item);
    }
}