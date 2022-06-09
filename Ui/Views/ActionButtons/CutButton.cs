using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class CutButton : ActionButton
{
    public CutButton()
    {
        _icon.Source = ResourcesIconsCompressed.CutCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        _main.ActionView.CopiedXaml.Clear();
        _main.ActionView.CutXaml.Clear();
        foreach (var item in _main.ActionView.SelectedXaml)
        {
            _main.ActionView.CopiedXaml.Add(item);
            _main.ActionView.CutXaml.Add(item);
        }
    }
}