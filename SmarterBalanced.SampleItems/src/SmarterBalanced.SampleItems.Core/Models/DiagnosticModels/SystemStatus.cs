using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SmarterBalanced.SampleItems.Core.Models.DiagnosticModels
{
    public class SystemStatus : BaseDiagnostic
    {
        [XmlArray(ElementName = "fileSystems")]
        public List<FilesystemStatus> FileSystems { get; set; }
    }
}
