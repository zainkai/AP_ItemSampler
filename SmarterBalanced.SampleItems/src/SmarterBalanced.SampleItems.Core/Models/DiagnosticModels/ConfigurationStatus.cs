using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Models.DiagnosticModels
{
    public class ConfigurationStatus : BaseDiagnostic
    {
        [XmlElement(ElementName = "S3-Content-Bucket")]
        public string ContentBucket { get; set; }

        [XmlElement(ElementName = "AWS-region")]
        public string AWSRegion { get; set; }
    }
}
