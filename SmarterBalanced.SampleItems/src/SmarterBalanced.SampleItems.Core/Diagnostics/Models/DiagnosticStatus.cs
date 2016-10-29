using System;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Diagnostics.Models
{
    [XmlType(TypeName = "status")]
    public class DiagnosticStatus : BaseDiagnostic
    {
        [XmlAttribute(AttributeName = "level")]
        public int Level { get; set; }

        [XmlAttribute(AttributeName = "unit")]
        public string Unit { get; set; }

        [XmlElement(ElementName = "system")]
        public SystemStatus SystemStatus { get; set; }

        [XmlElement(ElementName = "configuration")]
        public ConfigurationStatus ConfigurationStatus { get; set; }

        [XmlElement(ElementName = "database")]
        public AccessStatus AccessStatus { get; set; }

        [XmlElement(ElementName = "providers")]
        public DependencyStatus DependencyStatus { get; set; }

        [XmlIgnore]
        public DateTime CreationTime { get; set; }

        [XmlAttribute(AttributeName = "time")]
        public string Time
        {
            get
            {
                return CreationTime.ToUniversalTime().ToString("YYYY-MM-DDTHH:mm:ssZ") ?? null;
            }
        }
    }
}
