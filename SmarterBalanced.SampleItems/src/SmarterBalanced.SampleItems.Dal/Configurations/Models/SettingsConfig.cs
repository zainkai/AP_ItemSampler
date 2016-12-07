namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
{
    public class SettingsConfig
    {
        public string ContentItemDirectory { get; set; }
        public string ContentRootDirectory { get; set; }
        public string AccommodationsXMLPath { get; set; }
        public string InteractionTypesXMLPath { get; set; }
        public string AwsRegion { get; set; }
        public string AwsS3Bucket { get; set; }
        public string AwsS3ContentFilename { get; set; }
        public bool UseS3ForContent { get; set; }
        public string ItemViewerServiceURL { get; set; }
        public string AwsInstanceTag { get; set; }
        public string StatusUrl { get; set; }
        public string AccessibilityCookie { get; set; }
        public string ISAAPUrlParam { get; set; }
    }
}
