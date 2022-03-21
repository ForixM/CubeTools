using System;
using System.Collections.Generic;
using System.Configuration;

namespace ConfigLoader;

public static partial class ConfigLoader
{
    public static List<Tuple<string, string>> GetAllSettings()
    {
        var res = new List<Tuple<string, string>>();
        try
        {
            foreach (var key in AppSettings.AllKeys)
                if (key != null && AppSettings[key] != null)
                    res.Add(Tuple.Create(key, AppSettings[key])!);
        }
        catch (ConfigurationErrorsException)
        {
            // TODO Add logs
        }

        return res;
    }

    public static string GetOneSetting(string key)
    {
        if (string.IsNullOrEmpty(key))
            return "";
        var value = AppSettings[key];
        if (string.IsNullOrEmpty(value))
            return "";
        return value;
    }
}