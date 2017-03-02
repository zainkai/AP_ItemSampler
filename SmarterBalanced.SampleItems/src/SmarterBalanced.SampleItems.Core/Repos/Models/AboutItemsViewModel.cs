using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;


namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AboutItemsViewModel
    {
        public ImmutableArray<InteractionType> InteractionTypes { get; set; }

        public string ItemUrl { get; set; }

        public string SelectedInteractionTypeCode { get; set; }
        
        public AboutThisItemViewModel AboutThisItemViewModel { get; set; }

        public AboutItemsViewModel(
            ImmutableArray<InteractionType> interactionTypes,
            string itemUrl,
            string selectedCode,
            AboutThisItemViewModel aboutThisItemViewModel)
        {
            InteractionTypes = interactionTypes;
            ItemUrl = itemUrl;
            SelectedInteractionTypeCode = selectedCode ?? interactionTypes.FirstOrDefault()?.Code;
            AboutThisItemViewModel = aboutThisItemViewModel;
        }
    }
}