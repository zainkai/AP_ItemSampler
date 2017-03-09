using System.Linq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using Microsoft.Extensions.Logging;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AboutItemsRepo : ItemViewRepo, IAboutItemsRepo
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;

        public AboutItemsRepo(SampleItemsContext context, ILoggerFactory loggerFactory): base(context, loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<AboutItemsRepo>();
        }

        /// <summary>
        /// Constructs an AboutItemsViewModel with an item that matches
        /// the first interactionType in context.
        /// </summary>
        public AboutItemsViewModel GetAboutItemsViewModel()
        {
            var interactionTypes = context.InteractionTypes;
            string interactionTypeCode = interactionTypes.FirstOrDefault()?.Code;

            var model = GetAboutItemsViewModel(interactionTypeCode);

            return model;
        }

        /// <summary>
        /// Constructs and AboutItemsViewModel with an item that matches 
        /// the given interactionType.
        /// </summary>
        public AboutItemsViewModel GetAboutItemsViewModel(string interactionTypeCode)
        {
            SampleItem sampleItem = context.SampleItems
                .Where(i => i.Grade != GradeLevels.NA && i.InteractionType != null)
                .OrderBy(i => (int)i.Grade)
                .FirstOrDefault(item => item.InteractionType.Code == interactionTypeCode);

            if (sampleItem == null)
            {
                return null;
            }

            string itemURL = GetItemViewerUrlSingleItem(sampleItem);

            AboutThisItemViewModel aboutThisItemViewModel = GetAboutThisItemViewModel(sampleItem);
            AboutItemsViewModel model = new AboutItemsViewModel(
                interactionTypes: context.InteractionTypes,
                itemUrl: itemURL,
                selectedCode: interactionTypeCode,
                aboutThisItemViewModel: aboutThisItemViewModel);

            return model;
        }

        /// <summary>
        /// Constructs an ItemViewerService URL for a single item 
        /// </summary>
        private string GetItemViewerUrlSingleItem(SampleItem sampleItem)
        {
            if (sampleItem == null)
            {
                return null;
            }

            string baseUrl = context.AppSettings.SettingsConfig.ItemViewerServiceURL;
            return $"{baseUrl}/items?ids={sampleItem.ToString()}";
        }

    }

}
