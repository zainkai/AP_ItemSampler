using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Models.DiagnosticModels
{
    [XmlType(TypeName = "filesystem")]
    public class FilesystemStatus
    {
        [XmlIgnore]
        public int StatusRating { get; set; }

        [XmlAttribute(AttributeName = "freeSpace")]
        public string FreeSpace { get; set; }

        [XmlAttribute(AttributeName = "mountPoint")]
        public string MountPoint { get; set; }

        [XmlAttribute(AttributeName = "percentFreeSpace")]
        public string PercentFreeSpace { get; set; }

        [XmlAttribute(AttributeName = "totalSpace")]
        public string TotalSpace { get; set; }

        [XmlAttribute(AttributeName = "fsType")]
        public string FilesystemType { get; set; }

        [XmlElement(ElementName = "error")]
        public string ErrorMessage { get; set; }
    }
}
