using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class InteractionTypeTranslations
    {
        public static InteractionType ToInteractionType(this XElement elem)
        {
            var interactionType = new InteractionType(
                code: (string)elem.Element("Code"),
                label: (string)elem.Element("Label"),
                order: elem.Element("Order") == null ? 0 : (int)elem.Element("Order"));

            return interactionType;
        }

        public static List<InteractionType> ToInteractionTypes(this IEnumerable<XElement> itemElements)
        {
            var interactionTypes = itemElements
                .Select(i => i.ToInteractionType())
                .ToList();

            return interactionTypes;
        }

        public static List<InteractionFamily> ToInteractionFamilies(this XElement interactionTypesDoc)
        {
            var interactionFamilies = interactionTypesDoc
                .Element("Families")
                .Elements("Family")
                .Select(e => e.ToInteractionFamily())
                .ToList();

            return interactionFamilies;
        }

        public static InteractionFamily ToInteractionFamily(this XElement familyElement)
        {
            var interactionTypeCodes = familyElement
                    .Element("InteractionTypeCodes")
                    .Elements("Code")
                    .Select(e => (string)e)
                    .ToImmutableArray();

            var interactionFamily = new InteractionFamily(
                subjectCode: (string)familyElement.Element("SubjectCode"),
                interactionTypeCodes: interactionTypeCodes);

            return interactionFamily;
        }
    }

}
