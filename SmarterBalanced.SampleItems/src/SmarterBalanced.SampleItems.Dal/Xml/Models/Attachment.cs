using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class Attachment
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("pass")]
        public string Pass { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}
