using System.Collections.Generic;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Diagnostics.Models
{
    public class SystemStatus : BaseDiagnostic
    {
        [XmlArray(ElementName = "fileSystems")]
        public List<FilesystemStatus> FileSystems { get; set; }
    }
}
