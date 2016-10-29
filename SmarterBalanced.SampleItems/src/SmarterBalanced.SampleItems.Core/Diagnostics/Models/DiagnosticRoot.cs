using System.Collections.Generic;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Diagnostics.Models
{
    [XmlRoot(ElementName = "status")]
    public class DiagnosticRoot : BaseDiagnostic
    {
        [XmlIgnore]
        public new int StatusRating { get; set; }

        [XmlIgnore]
        public new string StatusText { get; set; }

        [XmlElement(ElementName = "status")]
        public List<DiagnosticStatus> DiagnosticStatuses { get; set; }
    }
}
