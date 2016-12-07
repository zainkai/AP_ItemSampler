using System.Collections.Generic;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilityResourceFamily
    {
        public List<string> Subjects { get; set; }

        public GradeLevels Grades { get; set; }

        public List<AccessibilityResource> Resources { get; set; }
    }
}
