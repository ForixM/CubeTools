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
        // CopiedXaml.Clear();
        // CutXaml.Clear();
        // foreach (var item in SelectedXaml) CopiedXaml.Add(item);
    }
}