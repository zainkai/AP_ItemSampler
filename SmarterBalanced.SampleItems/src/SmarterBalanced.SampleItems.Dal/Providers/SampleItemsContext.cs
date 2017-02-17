using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public sealed class SampleItemsContext
    {
        public ImmutableArray<ItemDigest> ItemDigests { get; }
        public ImmutableArray<ItemCardViewModel> ItemCards { get; }
        public ImmutableArray<InteractionType> InteractionTypes { get; }
        public ImmutableArray<Subject> Subjects { get; }
        public ImmutableArray<AccessibilityResourceFamily> AccessibilityResourceFamilies { get; }
        public AppSettings AppSettings { get; }

        public SampleItemsContext(
            ImmutableArray<ItemDigest> itemDigests,
            ImmutableArray<ItemCardViewModel> itemCards,
            ImmutableArray<InteractionType> interactionTypes,
            ImmutableArray<Subject> subjects,
            ImmutableArray<AccessibilityResourceFamily> accessibilityResourceFamilies,
            AppSettings appSettings)
        {
            ItemDigests = itemDigests;
            ItemCards = itemCards;
            InteractionTypes = interactionTypes;
            Subjects = subjects;
            AccessibilityResourceFamilies = accessibilityResourceFamilies;
            AppSettings = appSettings;
        }

        /// <summary>
        /// Used for testing or situations where not all properties need to be specified.
        /// </summary>
        public static SampleItemsContext Create(
            ImmutableArray<ItemDigest> itemDigests = default(ImmutableArray<ItemDigest>),
            ImmutableArray<ItemCardViewModel> itemCards = default(ImmutableArray<ItemCardViewModel>),
            ImmutableArray<InteractionType> interactionTypes = default(ImmutableArray<InteractionType>),
            ImmutableArray<Subject> subjects = default(ImmutableArray<Subject>),
            ImmutableArray<AccessibilityResourceFamily> accessibilityResourceFamilies = default(ImmutableArray<AccessibilityResourceFamily>),
            AppSettings appSettings = null)
        {
            var context = new SampleItemsContext(
                itemDigests: itemDigests,
                itemCards: itemCards,
                interactionTypes: interactionTypes,
                subjects: subjects,
                accessibilityResourceFamilies: accessibilityResourceFamilies,
                appSettings: appSettings);

            return context;
        }
    }
}
