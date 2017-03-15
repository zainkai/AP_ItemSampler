using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    [XmlRoot("root")]
    public class ItemPatchRoot
    {
        [XmlElement("row")]
        public List<ItemPatch> Patches { get; set; }
    }
}
