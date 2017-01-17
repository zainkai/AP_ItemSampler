using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
{
    public class SettingsConfig
    {
        public string ContentItemDirectory { get; set; }
        public string ContentRootDirectory { get; set; }
        public string AccommodationsXMLPath { get; set; }
        public string InteractionTypesXMLPath { get; set; }
        public string ClaimsXMLPath { get; set; }
        public string AwsRegion { get; set; }
        public string AwsS3Bucket { get; set; }
        public string AwsS3ContentFilename { get; set; }
        public bool UseS3ForContent { get; set; }
        public string ItemViewerServiceURL { get; set; }
        public string AwsClusterName { get; set; }
        public string StatusUrl { get; set; }
        public string AccessibilityCookie { get; set; }
        public string ISAAPUrlParam { get; set; }
        public Dictionary<string, string> AccessibilityTypeLabels { get; set; }
        public string BrowserWarningCookie { get; set; }
        public string UserAgentRegex { get; set; }
    }
}
