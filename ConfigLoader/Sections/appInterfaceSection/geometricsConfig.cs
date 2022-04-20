using System.Configuration;

namespace ConfigLoader.Sections.appInterfaceSection
{
    
    public class geometricsConfig : ConfigurationElementCollection
    {
        public geometricConfig this[int index]
        {
            get => BaseGet(index) as geometricConfig;
            set
            {
                if (BaseGet(index) != null) BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new geometricConfig this[string responseString]
        {
            get => (geometricConfig) BaseGet(responseString);
            set
            {
                if (BaseGet(responseString) != null) BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new geometricConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((geometricConfig) element).Name;
        }
    }
}
