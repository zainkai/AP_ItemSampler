using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class SubjectTranslation
    {
        public static ImmutableArray<Subject> ToSubjects(this XDocument claimsDoc, ImmutableArray<InteractionFamily> interactionFamilies)
        {
            var claimSubjects = claimsDoc
                .Element("Subjects")
                .Elements("Subject")
                .Select(s => s.ToSubject(interactionFamilies))
                .ToImmutableArray();

            return claimSubjects;
        }

        public static Subject ToSubject(this XElement subjectElement, ImmutableArray<InteractionFamily> interactionFamilies)
        {
            var code = (string)subjectElement.Element("Code");
            var family = interactionFamilies.Single(f => f.SubjectCode == code);

            var subject = new Subject(
                code: code,
                label: (string)subjectElement.Element("Label"),
                shortLabel: (string)subjectElement.Element("ShortLabel"),
                claims: subjectElement.Elements("Claim").ToClaims(),
                interactionTypeCodes: family.InteractionTypeCodes);

            return subject;
        }
    }
}
