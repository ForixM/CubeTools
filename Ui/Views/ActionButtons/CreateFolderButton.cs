using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Interactivity;
using ResourcesLoader;

namespace Ui.Views.ActionButtons;

public class CreateFolderButton : ActionButton
{
    public CreateFolderButton()
    {
        _icon.Source = ResourcesIconsCompressed.CreateFolderCompressed;
        OnClickEvent += OnClick;
    }
    
    private void OnClick(object? sender)
    {
        Debug.Print("Create Folder");
    }
}