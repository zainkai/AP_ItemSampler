﻿using System;
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
        public ImmutableArray<SampleItem> SampleItems { get; }
        public ImmutableArray<ItemCardViewModel> ItemCards { get; }
        public ImmutableArray<InteractionType> InteractionTypes { get; }
        public ImmutableArray<InteractionType> AboutInteractionTypes { get; }
        public ImmutableArray<AboutThisItemViewModel> AboutAllItems { get; }
        public ImmutableArray<Subject> Subjects { get; }
        public AppSettings AppSettings { get; }

        public SampleItemsContext(
            ImmutableArray<SampleItem> sampleItems,
            ImmutableArray<ItemCardViewModel> itemCards,
            ImmutableArray<InteractionType> interactionTypes,
            ImmutableArray<InteractionType> aboutInteractionTypes,
            ImmutableArray<Subject> subjects,
            ImmutableArray<AboutThisItemViewModel> aboutAllItems,
            AppSettings appSettings)
        {
            SampleItems = sampleItems;
            ItemCards = itemCards;
            InteractionTypes = interactionTypes;
            Subjects = subjects;
            AppSettings = appSettings;
            AboutInteractionTypes = aboutInteractionTypes;
            AboutAllItems = aboutAllItems;
        }

        /// <summary>
        /// Used for testing or situations where not all properties need to be specified.
        /// </summary>
        public static SampleItemsContext Create(
            ImmutableArray<SampleItem> sampleItems = default(ImmutableArray<SampleItem>),
            ImmutableArray<ItemCardViewModel> itemCards = default(ImmutableArray<ItemCardViewModel>),
            ImmutableArray<InteractionType> interactionTypes = default(ImmutableArray<InteractionType>),
            ImmutableArray<InteractionType> aboutInteractionTypes = default(ImmutableArray<InteractionType>),
            ImmutableArray<Subject> subjects = default(ImmutableArray<Subject>),
            ImmutableArray<AboutThisItemViewModel> aboutAllItems = default(ImmutableArray<AboutThisItemViewModel>),
            AppSettings appSettings = null)
        {
            var context = new SampleItemsContext(
                sampleItems: sampleItems,
                itemCards: itemCards,
                interactionTypes: interactionTypes,
                subjects: subjects,
                appSettings: appSettings,
                aboutAllItems: aboutAllItems,
                aboutInteractionTypes: aboutInteractionTypes);

            return context;
        }

        /// <summary>
        /// Gets a list of items that share a stimulus with the given item.
        /// Given item is returned as the first element of the list.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public List<SampleItem> GetAssociatedPerformanceItems(SampleItem item)
        {
            List<SampleItem> associatedStimulusDigests = SampleItems
                .Where(i => i.IsPerformanceItem &&
                    i.FieldTestUse != null &&
                    i.AssociatedStimulus == item.AssociatedStimulus &&
                    i.Grade == item.Grade && i.Subject?.Code == item.Subject?.Code)
                .OrderBy(i => i.FieldTestUse?.Section)
                .ThenBy(i => i.FieldTestUse?.QuestionNumber)
                .ToList();

            return associatedStimulusDigests;

        }
        public SampleItem GetSampleItem(int bankKey, int itemKey)
        {
            return SampleItems.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);

        }

    }
}
