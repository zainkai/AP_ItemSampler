using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Models.Configurations
{
    public class SettingsConfig
    {
        public string ContentItemDirectory { get; set; }
        public string ContentRootDirectory { get; set; }

        public string AccommodationsXMLPath { get; set; }

        public string AwsRegion { get; set; }
        public string AwsS3Bucket { get; set; }
        public string ItemViewerServiceURL { get; set; }

    }
}
