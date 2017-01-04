using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class RubricList
    {
        [XmlElement("rubric")]
        public List<RubricEntry> Rubrics { get; set; }

        [XmlElement("samplelist")]
        public List<RubricSample> RubricSamples { get; set; }
    }
}
