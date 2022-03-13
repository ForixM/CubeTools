using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Onedrive
{
    [DataContract]
    public class OneArboresence
    {
        [DataMember(Name = "@odata.context")]
        public string context
        {
            get;
            set;
        }

        [DataMember(Name = "@odata.count")]
        public int count
        {
            get;
            set;
        }

        [DataMember(Name = "value")]
        public IList<OneItem> value
        {
            get;
            set;
        }

        // public override string ToString()
        // {
        //     string disp = "context=" + context + "\ncount=" + count + "\nvalues=\n";
        //     foreach (OneItem oneItem in value)
        //     {
        //         disp += "{\n";
        //         disp += oneItem.ToString();
        //         disp += "}\n";
        //     }
        //     return disp;
        // }
    }
}