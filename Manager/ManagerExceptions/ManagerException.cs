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
            // READONLY Field
            ErrorType = "CubeTools crashed";
            CriticalLevel = "Low-Critical";
            // Modified
            Errorstd = "Unidentified error";
            ErrorMessage = "";
            ErrorFunction = "";
            ReloadFinalMessage();
        }

        public ManagerException(string std)
        {
            Errorstd = std;
            ErrorType = "CubeTools crashed";
            CriticalLevel = "Low-Critical";
            ErrorMessage = "";
            ErrorFunction = "";
            ReloadFinalMessage();
        }
        
        public ManagerException(string std, string message) : this(std)
        {
            ErrorMessage = message;
            ReloadFinalMessage();
            Console.Error.WriteLine(FinalMessage);
        }

        public ManagerException(string std, string message, string func) : this(std,message)
        {
            ErrorFunction = func;
            ReloadFinalMessage();
            Console.Error.WriteLine(FinalMessage);
        }

        public void ReloadFinalMessage()
        {
            FinalMessage = "########################################\n" +
                           $"###         {ErrorType}        ###\n" +
                           $" # {CriticalLevel} : {Errorstd}";
            if (!string.IsNullOrEmpty(ErrorMessage))
                FinalMessage += $" # error : {ErrorMessage}";
            if (!string.IsNullOrEmpty(ErrorFunction))
                FinalMessage += $" # error at : {ErrorFunction}";
        }
    }
}