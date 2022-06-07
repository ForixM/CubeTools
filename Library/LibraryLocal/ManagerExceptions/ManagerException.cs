using System;

namespace Library.ManagerExceptions
{
    public class ManagerException : Exception
    {
        public string ErrorMessage;
        private string ErrorFunction;
        public Level Level;
        public string Errorstd;
        public string ErrorType;

        public string FinalMessage;

        public ManagerException()
        {
            // READONLY Field : Modify directly parameters of exception
            ErrorType = "CubeTools crashed";
            Level = Level.Normal;
            // Modified
            Errorstd = "  # ??? #  ";
            ErrorMessage = "  # ??? #  ";
            ErrorFunction = "  # ??? #  ";
            FinalMessage = "";
            ReloadFinalMessage();
        }

        public ManagerException(string std = "Unknown error", Level level = Level.Normal, string type = "CubeTools crashed")
        {
            ErrorType = type;
            Level = level;
            Errorstd = std;
            ErrorMessage = "UNKNOWN";
            ErrorFunction = "UNKNOWN";
            FinalMessage = "";
            ReloadFinalMessage();
        }

        public ManagerException(string std, Level level, string type, string message)
        {
            ErrorType = type;
            Level = level;
            Errorstd = std;
            ErrorMessage = message;
            ErrorFunction = "UNKNOWN";
            FinalMessage = "";
            ReloadFinalMessage();
            LogErrors.LogErrors.LogWrite(FinalMessage, this);
        }

        public ManagerException(string std, Level level, string type, string message, string func, bool debug = false)
        {
            ErrorType = type;
            Level = level;
            Errorstd = std;
            ErrorMessage = message;
            ErrorFunction = func;
            FinalMessage = "";
            if (debug)
            {
                ReloadFinalMessage();
                LogErrors.LogErrors.LogWrite(FinalMessage, this);
            }

        }

        public void ReloadFinalMessage()
        {
            FinalMessage = "########################################\n" +
                           $"###         {ErrorType}       ###\n" +
                           $" # {Level} : {Errorstd}\n";
            if (!string.IsNullOrEmpty(ErrorMessage))
                FinalMessage += $" # error : {ErrorMessage}\n";
            if (!string.IsNullOrEmpty(ErrorFunction))
                FinalMessage += $" # error at : {ErrorFunction}\n";
            FinalMessage += "########################################\n";
        }
    }

    public enum Level
    {
        Info,
        Normal,
        High,
        Crash
    }
}