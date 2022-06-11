using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ResourcesLoader;
using Ui.Views.Actions;

namespace Ui.Views.ActionButtons;

public class CreateFileButton : ActionButton
{
    
    public CreateFileButton(int def) : base(def)
    {
        _icon.Source = ResourcesIconsCompressed.CreateCompressed;
        OnClickEvent += OnClick;
        ToolTip.SetTip(this, "Create File");
    }

    private void OnClick(object? sender)
    {
        Debug.Print("Create File");
        new CreateFile(_main).Show();
    }
    
}