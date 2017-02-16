using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class InteractionFamily
    {
        public string SubjectCode { get; }
        public ImmutableArray<string> InteractionTypeCodes { get; }

        public InteractionFamily(string subjectCode, ImmutableArray<string> interactionTypeCodes)
        {
            SubjectCode = subjectCode;
            InteractionTypeCodes = interactionTypeCodes;
        }

        public static InteractionFamily Create(ImmutableArray<string> interactionTypeCodes, string subjectCode = "" )
        {
            return new InteractionFamily(
                subjectCode: subjectCode, 
                interactionTypeCodes: interactionTypeCodes);
        }

        public static InteractionFamily Create(XElement familyElement)
        {
            var interactionTypeCodes = familyElement
                    .Element("InteractionTypeCodes")
                    .Elements("Code")
                    .Select(e => (string)e)
                    .ToImmutableArray();

            var interactionFamily = Create(
                subjectCode: (string)familyElement.Element("SubjectCode"),
                interactionTypeCodes: interactionTypeCodes);

            return interactionFamily;
        }

    }
}
