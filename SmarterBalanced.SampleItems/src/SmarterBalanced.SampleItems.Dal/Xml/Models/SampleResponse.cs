using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class SampleResponse
    {
        [XmlAttribute("purpose")]
        public string Purpose { get; set; }

        [XmlAttribute("scorepoint")]
        public string ScorePoint { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("samplecontent")]
        public string SampleContent { get; set; }
    }
}
