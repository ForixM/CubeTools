using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class CreateFolderButton : ActionButton
{
    public CreateFolderButton(ClientUI main, int def) : base(main, def)
    {
        _icon.Source = ResourcesIconsCompressed.CreateFolderCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Create Folder");
    }
    
    private void OnClick(object? sender)
    {
        new CreateFolder(_main).Show();
    }
}