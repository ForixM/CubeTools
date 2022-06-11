using System.Diagnostics;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class SnapdropButton : ActionButton
{
    public SnapdropButton(ClientUI main, int def) : base(main, def)
    {
        _icon.Source = ResourcesIconsCompressed.SnapDropCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        var uri = "https://www.snapdrop.net";
        var psi = new System.Diagnostics.ProcessStartInfo();
        psi.UseShellExecute = true;
        psi.FileName = uri;
        System.Diagnostics.Process.Start(psi);
    }
}