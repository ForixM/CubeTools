using System;

namespace Manager.ManagerExceptions
{
    public class ManagerException : Exception
    {
        public string Errorstd;
        public string ErrorType;
        public string CriticalLevel;
        public static string ErrorMessage;
        public static string ErrorFunction;
        
        public string FinalMessage;

        public ManagerException()
        {
            // READONLY Field : Modify directly parameters of exception
            ErrorType = "CubeTools crashed";
            CriticalLevel = "Low-Critical";
            // Modified
            Errorstd = "  # ??? #  ";
            ErrorMessage = "  # ??? #  ";
            ErrorFunction = "  # ??? #  ";
            ReloadFinalMessage();
        }

        public ManagerException(string std = "Crash", string level = "Low-Critical", string type = "CubeTools crashed") : this()
        {
            ErrorType = type;
            CriticalLevel = level;
            Errorstd = std;
            ReloadFinalMessage();
        }
        
        public ManagerException(string std,string level, string type, string message) : this()
        {
            ErrorType = type;
            CriticalLevel = level;
            Errorstd = std;
            ErrorMessage = message;
            ReloadFinalMessage();
            Console.Error.WriteLine(FinalMessage);
        }

        public ManagerException(string std, string level, string type, string message, string func) : this()
        {
            ErrorType = type;
            CriticalLevel = level;
            Errorstd = std;
            ErrorMessage = message;
            ErrorFunction = func;
            ReloadFinalMessage();
            Console.Error.WriteLine(FinalMessage);
        }

        public void ReloadFinalMessage()
        {
            FinalMessage = "########################################\n" +
                           $"###         {ErrorType}       ###\n" +
                           $" # {CriticalLevel} : {Errorstd}\n";
            if (!string.IsNullOrEmpty(ErrorMessage))
                FinalMessage += $" # error : {ErrorMessage}\n";
            if (!string.IsNullOrEmpty(ErrorFunction))
                FinalMessage += $" # error at : {ErrorFunction}\n";
            FinalMessage += "########################################\n";
        }
    }
}