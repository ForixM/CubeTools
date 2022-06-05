using System;
using Library.ManagerExceptions;
using Ui.Views.Error.Information;
using Ui.Views.Error.Refresh;
using Ui.Views.Error.Replace;

namespace Ui.Views.Error
{

    public static class ErrorConverter
    {
        public static void SetContainer(ErrorBase reference, ManagerException exception, PopUpAction action)
        {
            reference.Container = action switch
            {
                PopUpAction.INFO => new ErrorInfo(reference),
                PopUpAction.REFRESH => new RefreshAction(reference),
                PopUpAction.REPLACE => new ReplaceAction(reference),
                _ => reference.Container
            };
        }
    }

    public enum PopUpAction
    {
        REFRESH,
        REPLACE,
        INFO
    }
}