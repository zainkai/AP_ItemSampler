using System.Collections.Generic;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilityResourceFamily
    {
        public ImmutableArray<string> Subjects { get; }
        public GradeLevels Grades { get; }
        public ImmutableArray<AccessibilityResource> Resources { get; }

        public AccessibilityResourceFamily(
            ImmutableArray<string> subjects,
            GradeLevels grades,
            ImmutableArray<AccessibilityResource> resources)
        {
            Subjects = subjects;
            Grades = grades;
            Resources = resources;
        }
    }
}
