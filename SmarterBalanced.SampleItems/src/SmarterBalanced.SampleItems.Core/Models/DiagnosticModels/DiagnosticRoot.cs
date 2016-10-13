using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Models.DiagnosticModels
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
