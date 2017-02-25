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

        public AboutItemsRepo(SampleItemsContext context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<AboutItemsRepo>();
        }

        public AboutItemsViewModel GetAboutItemsViewModel()
        {
            var interactionTypes = context.InteractionTypes;
            string itemURL = GetItemViewerUrl(interactionTypes.FirstOrDefault()?.Code);
            AboutItemsViewModel model = new AboutItemsViewModel(interactionTypes, itemURL);
            return model;
        }

        public string GetItemViewerUrl(string interactionTypeCode)
        {
            if (string.IsNullOrEmpty(interactionTypeCode))
            {
                return null;
            }

            var itemDigest = context.SampleItems
                .Where(i => i.Grade != GradeLevels.NA && i.InteractionType != null)
                .OrderBy(i => (int)i.Grade)
                .FirstOrDefault(item => item.InteractionType.Code == interactionTypeCode);

            return GetItemViewerUrl(itemDigest);
        }

    }

}
