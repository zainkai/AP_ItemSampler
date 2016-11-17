using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    /// <summary>
    /// Object representation of a metadata.xml file
    /// </summary>
    [XmlRoot("metadata")]
    public class ItemMetadata
    {
        [XmlElement("smarterAppMetadata", Namespace = "http://www.smarterapp.org/ns/1/assessment_item_metadata")]
        public SmarterAppMetadataXmlRepresentation Metadata { get; set; }
    }
}
