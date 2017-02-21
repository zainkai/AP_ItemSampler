using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class MergedAccessibilityFamily
    {
        public ImmutableArray<string> Subjects { get; }
        public GradeLevels Grades { get; }
        public ImmutableArray<AccessibilityResource> Resources { get; }

        public MergedAccessibilityFamily(
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
