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
        public string Type { get;}
        public ImmutableArray<string> InteractionTypeCodes { get; }

        public InteractionFamily(string subjectCode, ImmutableArray<string> interactionTypeCodes, string type)
        {
            SubjectCode = subjectCode;
            InteractionTypeCodes = interactionTypeCodes;
            Type = type;
        }

        public static InteractionFamily Create(ImmutableArray<string> interactionTypeCodes, string subjectCode = "", string type = "" )
        {
            return new InteractionFamily(
                subjectCode: subjectCode, 
                interactionTypeCodes: interactionTypeCodes,
                type: type);
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
                type: (string)familyElement.Element("Type"),
                interactionTypeCodes: interactionTypeCodes);

            return interactionFamily;
        }

    }
}
