using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Diagnostics.Models
{
    public class ConfigurationStatus : BaseDiagnostic
    {
        [XmlElement(ElementName = "S3-Content-Bucket")]
        public string ContentBucket { get; set; }

        [XmlElement(ElementName = "AWS-region")]
        public string AWSRegion { get; set; }
    }
}
