using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class ItemViewRepo : IItemViewRepo
    {
        private readonly SampleItemsContext context;
        private readonly ILogger logger;

        public ItemViewRepo(SampleItemsContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            logger = loggerFactory.CreateLogger<ItemViewRepo>();
        }

        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return context.ItemDigests.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        public ItemCardViewModel GetItemCardViewModel(int bankKey, int itemKey)
        {
            return context.ItemCards.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
        protected string GetItemViewerUrl(ItemDigest digest)
        {
            if (digest == null)
            {
                return string.Empty;
            }

            string baseUrl = context.AppSettings.SettingsConfig.ItemViewerServiceURL;
            return $"{baseUrl}/item/{digest.BankKey}-{digest.ItemKey}";
        }

        public ItemViewModel GetItemViewModel(
            int bankKey,
            int itemKey,
            string[] iSAAPCodes,
            Dictionary<string, string> cookiePreferences)
        {
            var itemDigest = GetItemDigest(bankKey, itemKey);
            var itemCardViewModel = GetItemCardViewModel(bankKey, itemKey);
            if (itemDigest == null || itemCardViewModel == null)
            {
                return null;
            }

            var aboutItem = new AboutThisItemViewModel(
                rubrics: itemDigest.Rubrics,
                itemCard: itemCardViewModel);

            var groups = itemDigest.AccessibilityResourceGroups.ApplyPreferences(iSAAPCodes, cookiePreferences);

            var itemViewModel = new ItemViewModel(
                itemViewerServiceUrl: GetItemViewerUrl(itemDigest),
                accessibilityCookieName: context.AppSettings.SettingsConfig.AccessibilityCookie,
                aboutItemVM: aboutItem,
                accResourceGroups: groups);

            return itemViewModel;
        }

    }

}
