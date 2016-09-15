using SmarterBalanced.SampleItems.Dal.Models.XMLRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    /// <summary>
    /// Object representation of a metadata.xml file
    /// </summary>
    [XmlRoot("metadata")]
    public class ItemMetadata
    {
        [XmlElement("smarterAppMetadata", Namespace = "http://www.smarterapp.org/ns/1/assessment_item_metadata")]
        public SmarterAppMetadataXmlRepresentation metadata { get; set; }
    }
}
