using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Models.DiagnosticModels
{
    public class DependencyStatus : BaseDiagnostic
    {
        [XmlElement(ElementName = "itemviewerservice-API-HTTP-status")]
        public string ItemViewerServiceStatus { get; set; }

        [XmlElement(ElementName = "content-package")]
        public string ContentPackage { get; set; }

        [XmlElement(ElementName = "accessibility-accommodation-configurations")]
        public string AccessibilityConfiguration { get; set; }
    }
}
