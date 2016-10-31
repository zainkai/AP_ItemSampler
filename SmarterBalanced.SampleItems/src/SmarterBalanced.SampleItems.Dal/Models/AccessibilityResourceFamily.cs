using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Gen = SmarterBalanced.SampleItems.Dal.Models.Generated;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    public class AccessibilityResourceFamily
    {
        public List<string> Codes { get; set; }

        public Gen.GradeType[] Grades { get; set; }

        public List<AccessibilityResource> Resources { get; set; }
    }
}
