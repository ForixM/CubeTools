using System.Configuration;

namespace ConfigLoader.Sections.appLauncherSection;

public class applicationsConfig : ConfigurationElementCollection
{
    public applicationConfig this[int index]
    {
        get
        {
            return base.BaseGet(index) as applicationConfig;
        }
        set
        {
            if (base.BaseGet(index) != null)
            {
                base.BaseRemoveAt(index);
            }
            this.BaseAdd(index, value);
        }
    }

    public new applicationConfig this[string responseString]
    {
        get { return (applicationConfig) BaseGet(responseString); }
        set
        {
            if(BaseGet(responseString) != null)
            {
                BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
            }
            BaseAdd(value);
        }
    }

    protected override System.Configuration.ConfigurationElement CreateNewElement()
    {
        return new applicationConfig();
    }

    protected override object GetElementKey(System.Configuration.ConfigurationElement element)
    {
        return ((applicationConfig)element).Name;
    }
}