using SmarterBalanced.SampleItems.Dal.Models.XMLRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    [XmlRoot("metadata")]
    public class ItemMetadata
    {
        [XmlElement("smarterAppMetadata")]
        public SmarterAppMetadataXmlRepresentation metadata { get; set; }
    }
}
