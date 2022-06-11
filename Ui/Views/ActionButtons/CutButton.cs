using Avalonia.Controls;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class CutButton : ActionButton
{
    public CutButton(ClientUI main, int def) : base(main, def)
    {
        _icon.Source = ResourcesIconsCompressed.CutCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Cut");
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