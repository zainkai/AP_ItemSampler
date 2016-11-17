using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
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
