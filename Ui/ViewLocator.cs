using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Ui.Views.MainWindow;
using Ui.Views.Remote;

namespace Ui
{
    public class ViewLocator : IDataTemplate
    {
        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ParentViewModelXaml", "View");
            var type = Type.GetType(name);

            if (type != null)
                return (Control) Activator.CreateInstance(type)!;
            return new TextBlock {Text = "Not Found: " + name};
        }

        public bool Match(object data) => data is MainWindow;
    }
}