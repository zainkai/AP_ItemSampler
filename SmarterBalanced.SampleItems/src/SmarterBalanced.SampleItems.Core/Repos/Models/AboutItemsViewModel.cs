using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AboutItemsViewModel
    {
        public ImmutableArray<InteractionType> InteractionTypes { get; set; }

        public string ItemUrl { get; set; }

        public AboutItemsViewModel(
            ImmutableArray<InteractionType> interactionTypes,
            string itemUrl)
        {
            InteractionTypes = interactionTypes;
            ItemUrl = itemUrl;
        }
    }
}