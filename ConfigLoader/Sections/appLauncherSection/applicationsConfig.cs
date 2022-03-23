using System.Configuration;

namespace ConfigLoader.Sections.appLauncherSection;

public class applicationsConfig : ConfigurationElementCollection
{
    public applicationConfig this[int index]
    {
        get => BaseGet(index) as applicationConfig;
        set
        {
            if (BaseGet(index) != null) BaseRemoveAt(index);
            BaseAdd(index, value);
        }
    }

    public new applicationConfig this[string responseString]
    {
        get => (applicationConfig) BaseGet(responseString);
        set
        {
            if (BaseGet(responseString) != null) BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
            BaseAdd(value);
        }
    }
    protected override ConfigurationElement CreateNewElement() => new applicationConfig();
    protected override object GetElementKey(ConfigurationElement element) => ((applicationConfig) element).Name;
}