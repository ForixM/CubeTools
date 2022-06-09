using System;
using Library.ManagerExceptions;
using Ui.Views.Error.Information;
using Ui.Views.Error.Refresh;
using Ui.Views.Error.Replace;

namespace Ui.Views.Error
{

    public static class ErrorConverter
    {
        public static void SetContainer(ErrorBase reference, ManagerException exception)
        {
            reference.Type = GetAction(exception);
            reference.Container.Children.Add(reference.Type switch
            {
                PopUpAction.INFO => new ErrorInfo(reference),
                PopUpAction.REFRESH => new RefreshAction(reference),
                PopUpAction.REPLACE => new ReplaceAction(reference),
                _ => reference.Container
            });
        }

        private static PopUpAction GetAction(ManagerException exception)
        {
            switch (exception)
            {
                case PathFormatException :
                case SystemErrorException :
                case AccessException :
                    return PopUpAction.INFO;
                case CorruptedPointerException :
                case CorruptedDirectoryException :
                case DiskNotReadyException :
                case PathNotFoundException :
                    return PopUpAction.REFRESH;
                case ConnectionLost :
                case ConnectionRefused :
                    return PopUpAction.CONNECTION_FAILURE;
                default:
                    return PopUpAction.INFO;
            }
        }
    }

    public enum PopUpAction
    {
        REFRESH,
        REPLACE,
        INFO,
        CONNECTION_FAILURE
    }
}