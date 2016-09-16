using SmarterBalanced.SampleItems.Dal.Models.XMLRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    /// <summary>
    /// Object representation of item-BANK-KEY.xml files
    /// </summary>
    [XmlRoot("itemrelease")]
    public class ItemContents
    {
        [XmlElement("item")]
        public ItemXmlFieldRepresentation Item { get; set; }
    }
}
