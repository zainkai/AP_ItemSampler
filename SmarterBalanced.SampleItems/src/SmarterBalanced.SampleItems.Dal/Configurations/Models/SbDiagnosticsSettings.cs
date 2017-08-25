using System;
using System.Collections.Generic;
using System.Text;

namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
{
    public class SbDiagnosticsSettings
    {
        public string AwsRegion { get; set; }
        public string AwsS3Bucket { get; set; }
        public string AwsClusterName { get; set; }
        public string StatusUrl { get; set; }
    }
}
