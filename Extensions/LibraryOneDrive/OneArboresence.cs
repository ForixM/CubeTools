using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LibraryOneDrive
{
    [DataContract]
    public class OneArboresence
    {
        [DataMember(Name = "@odata.context")] public string context { get; set; }

        [DataMember(Name = "@odata.count")] public int count { get; set; }

        [DataMember(Name = "value")] public IList<OneItem> items { get; set; }
        
    }
}