using SmarterBalanced.SampleItems.Dal.Models.XMLRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    public class ItemContents
    {
        [XmlElement("item")]
        ItemXmlFieldRepresentation item { get; set; }
    }
}
