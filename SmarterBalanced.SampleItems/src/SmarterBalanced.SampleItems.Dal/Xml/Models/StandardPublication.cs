using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    /// <summary>
    /// Represents the standard publication field for the item metadata xml
    /// </summary>
    public class StandardPublication
    {
        [XmlElement("Publication")]
        public string Publication { get; set; }

        [XmlElement("PrimaryStandard")]
        public string PrimaryStandard { get; set; }

        [XmlElement("SecondaryStandard")]
        public string SecondaryStandard { get; set; }
    }
}
