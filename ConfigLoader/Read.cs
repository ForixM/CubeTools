using System;
using System.Collections.Generic;
using System.Configuration;
using ConfigLoader.Sections.appLauncherSection;

namespace ConfigLoader;

public static partial class ConfigLoader
{
    public static List<Tuple<string, string>> GetAllSettings()
    {
        var res = new List<Tuple<string,string>>();
        var values = Enum.GetValues(typeof(selectedSection));
        // Default
        res.AddRange(GetAllSettingSection(selectedSection.AppSettingsSection, selectedCollection.None));
        // Adding AppLauncherSection
        res.AddRange(GetAllSettingSection(selectedSection.AppLauncherSection, selectedCollection.ApplicationsCollection));
        // Adding AppInterfaceSection
        res.AddRange(GetAllSettingSection(selectedSection.AppInterfaceSection, selectedCollection.GeometricsCollection));
        res.AddRange(GetAllSettingSection(selectedSection.AppInterfaceSection, selectedCollection.StyleCollection));
        // Return result
        return res;
    }
    public static List<Tuple<string, string>> GetAllSettingSection(selectedSection section, selectedCollection collection)
    {
        var res = new List<Tuple<string, string>>();
        try
        {
            switch (section)
            {
                case selectedSection.AppLauncherSection:
                    switch (collection)
                    {
                        case selectedCollection.ApplicationsCollection:
                            for (int i = 0; i < AppLauncher.Applications.Count; i++)
                                res.Add(Tuple.Create(AppLauncher.Applications[i].Name, AppLauncher.Applications[i].Value)!);
                            break;
                        default :
                            return new List<Tuple<string, string>>();
                    }
                    break;
                case selectedSection.AppInterfaceSection:
                    switch (collection)
                    {
                        case selectedCollection.StyleCollection:
                            for (int i = 0; i < AppInterface.Styles.Count; i++)
                                res.Add(Tuple.Create(AppInterface.Styles[i].Name, AppInterface.Styles[i].Value)!);
                            break;
                        case selectedCollection.GeometricsCollection:
                            for (int i = 0; i < AppInterface.Geometrics.Count; i++)
                                res.Add(Tuple.Create(AppInterface.Geometrics[i].Name, AppInterface.Geometrics[i].Value)!);
                            break;
                        default :
                            return new List<Tuple<string, string>>();
                    }
                    break;
                case selectedSection.AppSettingsSection :
                    foreach (var key in AppSettings.AllKeys)  
                        res.Add(Tuple.Create(key, AppSettings[key])!);
                    break;
            }
        }
        catch (ConfigurationErrorsException)
        {
            // TODO Add logs
        }

        return res;
    }

    public static string GetOneSetting(string key)
    {
        foreach (var tuple in GetAllSettings())
        {
            if (tuple.Item1 == key) return tuple.Item2;
        }

        return "";
    }
}