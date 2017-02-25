using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml.Linq;
using System.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilityFamily
    {
        public ImmutableArray<string> Subjects { get; }
        public GradeLevels Grades { get; }
        public ImmutableArray<AccessibilityFamilyResource> Resources { get; }

        public AccessibilityFamily(
            ImmutableArray<string> subjects,
            GradeLevels grades,
            ImmutableArray<AccessibilityFamilyResource> resources)
        {
            Subjects = subjects;
            Grades = grades;
            Resources = resources;
        }

        public static AccessibilityFamily Create(XElement elem)
        {
            var subjects = elem.Elements("Subject")
                .Select(s => (string)s.Element("Code"))
                .ToImmutableArray();

            var resources = elem.Elements("SingleSelectResource")
                .Select(r => AccessibilityFamilyResource.Create(r))
                .ToImmutableArray();

            var grades = elem.Elements("Grade")
                .Select(g => g.Value)
                .ToGradeLevels();

            var accFamily = new AccessibilityFamily(
                subjects: subjects,
                grades: grades,
                resources: resources);

            return accFamily;
        }
    }
}
