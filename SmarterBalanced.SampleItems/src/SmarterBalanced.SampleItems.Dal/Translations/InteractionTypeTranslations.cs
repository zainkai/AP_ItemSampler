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
        public static ImmutableArray<InteractionType> ToInteractionTypes(this XElement elem)
        {
            var interactionTypes = elem
                .Element("Items")
                .Elements("Item")
                .Select(i => i.ToInteractionType())
                .ToImmutableArray();

            return interactionTypes;
        }

        private static InteractionType ToInteractionType(this XElement elem)
        {
            var interactionType = new InteractionType(
                code: (string)elem.Element("Code"),
                label: (string)elem.Element("Label"),
                order: elem.Element("Order") == null ? 0 : (int)elem.Element("Order"));

            return interactionType;
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
