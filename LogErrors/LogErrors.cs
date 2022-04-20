// System

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
// Library


namespace LogErrors
{
    public class LogErrors
    {
        public static readonly string LogPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Logs";
        public static string FilePath = String.Empty;
        private static TextWriter? _fs;
        
        public static void LogWrite(string message, Exception e)
        {
            var time = DateTime.Now.ToString(CultureInfo.CurrentCulture) + "\n";
            var text = "";
            text += time + ": " + message + "\n";
            text += e.Message + "\n";
            
            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);
            
            if (!File.Exists(FilePath))
            {
                time = time.Replace('/', '_');
                time = time.Replace(':', '_');
                time = time.Trim(new []{'A', 'M', 'P'});
                foreach (var c in Path.GetInvalidPathChars())
                {
                    if (time.Contains(c))
                        time = time.Replace(c, ' ');
                }
                FilePath = LogPath + '/' + time + ".txt";
            }

            try
            {
                _fs = new StreamWriter(FilePath, true);
                _fs.WriteLine(text);
                _fs.Close();
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
            }
        }
    }
}