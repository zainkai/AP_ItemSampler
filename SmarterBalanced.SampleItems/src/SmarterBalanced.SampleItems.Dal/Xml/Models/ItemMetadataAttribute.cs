using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class ItemMetadataAttribute
    {
        [XmlAttribute("attid")]
        public string Code { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("val")]
        public string Value { get; set; }
    }
}
