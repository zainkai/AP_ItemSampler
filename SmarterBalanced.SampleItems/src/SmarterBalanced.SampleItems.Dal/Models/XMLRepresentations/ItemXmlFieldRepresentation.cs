using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Models.XMLRepresentations
{
    /// <summary>
    /// Represents the item element in an item-BANK-KEY.xml file
    /// </summary>
    public class ItemXmlFieldRepresentation
    {
        [XmlAttribute("bankkey")]
        public int bank;

        [XmlAttribute("id")]
        public int itemKey { get; set; }
    }
}
