using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    public class AccessibilityResource
    {
        public string Code { get; set; }

        public int Order { get; set; }

        public string DefaultSelection { get; set; }

        public List<AccessibilitySelection> Selections { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }
    }
}
