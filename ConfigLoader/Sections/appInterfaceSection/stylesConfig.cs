using System.Configuration;

namespace ConfigLoader.Sections.appInterfaceSection
{
    
    public class stylesConfig : ConfigurationElementCollection
    {
        public styleConfig this[int index]
        {
            get => BaseGet(index) as styleConfig;
            set
            {
                if (BaseGet(index) != null) BaseRemoveAt(index);
                BaseAdd(index, value);
            }
        }

        public new stylesConfig this[string responseString]
        {
            get => (stylesConfig) BaseGet(responseString);
            set
            {
                if (BaseGet(responseString) != null) BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new styleConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((styleConfig) element).Name;
        }
    }
}
