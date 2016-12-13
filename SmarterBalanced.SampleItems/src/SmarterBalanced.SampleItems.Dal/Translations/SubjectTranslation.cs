using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class SubjectTranslation
    {
        public static List<Subject> ToSubjects(this XDocument claimsDoc, List<InteractionFamily> interactionFamilies)
        {
            List<Subject> claimSubjects = claimsDoc
                .Element("Subjects")
                .Elements("Subject")
                .ToSubjects(interactionFamilies);

            return claimSubjects;
        }

        public static List<Subject> ToSubjects(this IEnumerable<XElement> subjectElements, List<InteractionFamily> interactionFamilies)
        {
            List<Subject> claimSubjects = subjectElements
                .Select(s => s.ToSubject(interactionFamilies))
                .ToList();

            return claimSubjects;
        }

        public static Subject ToSubject(this XElement subjectElement, List<InteractionFamily> interactionFamilies)
        {
            var code = (string)subjectElement.Element("Code");
            var family = interactionFamilies.Single(f => f.SubjectCode == code);

            // TODO: add labels to the claims config doc?
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
