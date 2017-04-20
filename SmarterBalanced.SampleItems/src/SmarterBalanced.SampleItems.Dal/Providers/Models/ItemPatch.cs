using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class ItemPatch
    {
        [XmlElement("ItemId")]
        public int ItemId { get; set; }

        [XmlElement("Claim")]
        public string Claim { get; set; }

        [XmlElement("Target")]
        public string Target { get; set; }

        [XmlElement("TargetDesc")]
        public string TargetDescription { get; set; }

        [XmlElement("CCSSDesc")]
        public string CCSSDescription { get; set; }

        [XmlElement("Section")]
        public string QuestionNumber { get; set; }
    }
}
