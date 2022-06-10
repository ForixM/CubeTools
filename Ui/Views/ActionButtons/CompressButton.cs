using System.Linq;
using Avalonia.Controls;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class CompressButton : ActionButton
{
    public CompressButton()
    {
        _icon.Source = ResourcesIconsCompressed.CompressionCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Compress");
    }

    private void OnClick(object sender)
    {
        if (_main.ActionView.SelectedXaml.Count == 0) return;

        var archives = _main.ActionView.SelectedXaml.Where(ft =>  ft.Pointer.Type is "zip" or "7z").Select(item => item.Pointer);
        var others = _main.ActionView.SelectedXaml.Where(ft =>  ft.Pointer.Type is not "zip" && ft.Pointer.Type is not "7z").Select(item => item.Pointer);

        // Opening extract for archives
        var archiveTypes = archives.ToList();
        if (archiveTypes.Any()) new Extract(_main, archiveTypes).Show();
            
        // Extracting all archives
        var allTypes = others.ToList();
        if (allTypes.Any()) new Compress(_main, allTypes).Show();
    }
}