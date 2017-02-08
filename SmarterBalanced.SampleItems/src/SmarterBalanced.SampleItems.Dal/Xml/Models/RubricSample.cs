using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class RubricSample
    {
        [XmlAttribute("maxval")]
        public string MaxValue { get; set; }

        [XmlAttribute("minval")]
        public string MinValue { get; set; }

        [XmlElement("sample")]
        public List<SampleResponse> SampleResponses { get; set; }
    }
}
