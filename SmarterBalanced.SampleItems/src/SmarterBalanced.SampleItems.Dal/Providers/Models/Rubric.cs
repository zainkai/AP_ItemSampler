using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class Rubric
    {
        public string Language { get; set; }

        public List<RubricEntry> RubricEntries { get; set; }

        public List<RubricSample> Samples { get; set; }
    }
}
