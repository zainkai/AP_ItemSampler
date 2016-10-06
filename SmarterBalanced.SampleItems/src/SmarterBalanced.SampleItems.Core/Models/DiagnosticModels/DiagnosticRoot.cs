using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Models.DiagnosticModels
{
    [XmlRoot(ElementName = "status")]
    public class DiagnosticRoot
    {
        [XmlElement(ElementName = "status")]
        public List<DiagnosticStatus> DiagnosticStatuses { get; set; }
    }
}
