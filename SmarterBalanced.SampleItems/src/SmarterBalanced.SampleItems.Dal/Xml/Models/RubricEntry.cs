using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class RubricEntry
    {
        [XmlAttribute("scorepoint")]
        public string Scorepoint { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("val")]
        public string Value { get; set; }
    }
}
