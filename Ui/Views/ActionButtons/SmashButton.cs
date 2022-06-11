using System.Diagnostics;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class SmashButton : ActionButton
{
    public SmashButton(int def) : base(def)
    {
        _icon.Source = ResourcesIconsCompressed.SmashCompressed;
        OnClickEvent += OnClick;
    }

    private void OnClick(object sender)
    {
        var uri = "https://www.fromsmash.com";
        var psi = new System.Diagnostics.ProcessStartInfo();
        psi.UseShellExecute = true;
        psi.FileName = uri;
        System.Diagnostics.Process.Start(psi);
        /*
        const string uri = "https://www.fromsmash.com";
        var psi = new System.Diagnostics.ProcessStartInfo
        {
            FileName = uri
        };
        System.Diagnostics.Process.Start(psi);
        */
    }
}