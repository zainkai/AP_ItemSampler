using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class InteractionTypeTranslations
    {

        /// <summary>
        /// Translates IEnumerable of XElement into a list of InteractionTypes.
        /// </summary>
        /// <param name="itemElements"></param>
        /// <returns></returns>
        public static IList<InteractionType> ToInteractionTypes(this IEnumerable<XElement> itemElements)
        {
            IList<InteractionType> interactionTypes = itemElements
                .Select(i => new InteractionType
                {
                    Code = (string)i.Element("Code"),
                    Label = (string)i.Element("Label"),
                    Order = (i.Element("Order") == null) ? 0 : (int)i.Element("Order")
                }).ToList();

            return interactionTypes;
        }

        /// <summary>
        /// Given a list of InteractionTypes, translates XElement of InteractionFamilies
        /// into InteractionFamilies with InteractionTypes.
        /// </summary>
        /// <param name="typesDoc"></param>
        /// <param name="interactionTypes"></param>
        /// <returns></returns>
        public static IList<InteractionFamily> ToInteractionFamilies(this XElement typesDoc, IList<InteractionType> interactionTypes)
        {
            IList<InteractionFamily> interactionFamilies = typesDoc
                .Element("Families")
                .Elements("Family")
                .Select(i => new InteractionFamily
                {
                    Code = (string)i.Element("Code"),
                    InteractionTypes = i.Elements("Items")
                        .ToInteractionTypes()
                        .MergeInteractionTypes(interactionTypes)
                }).ToList();

            return interactionFamilies;
        }

        /// <summary>
        /// Merges two ILists of InteractionTypes. 
        /// </summary>
        /// <param name="familyTypes"></param>
        /// <param name="fullTypes"></param>
        /// <returns></returns>
        public static List<InteractionType> MergeInteractionTypes(this IList<InteractionType> familyTypes, IList<InteractionType> fullTypes)
        {
            List<InteractionType> interactionTypes = new List<InteractionType>();

            foreach(InteractionType famType in familyTypes)
            {
                InteractionType it = fullTypes.SingleOrDefault(i => i.Code == famType.Code);
                InteractionType newInteractionType = new InteractionType
                {
                    Code = famType.Code,
                    Order = famType.Order,
                    Label = it?.Label
                };

                interactionTypes.Add(newInteractionType);
            }

            return interactionTypes;
        }

    }

}
