using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos
{
    public class ItemViewRepo : IItemViewRepo
    {
        private SampleItemsContext context;

        public ItemViewRepo(SampleItemsContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Gets AppSettings
        /// </summary>
        /// <returns></returns>
        public AppSettings AppSettings
        {
            get
            {
                return context.AppSettings;
            }
        }

        /// <summary>
        /// Retrieves an ItemDigest matching the given bankKey and itemKey.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemDigest object.</returns>
        public Task<ItemDigest> GetItemDigestAsync(int bankKey, int itemKey)
        {
            return Task.Run(() => context.ItemDigests.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey));
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
        /// <param name="digest"></param>
        /// <returns>a string URL.</returns>
        private string GetItemViewerUrl(ItemDigest digest, string iSAAPcode)
        {
            if (digest == null)
            {
                return string.Empty;
            }

            string baseUrl = context.AppSettings.SettingsConfig.ItemViewerServiceURL;
            return $"{baseUrl}/item/{digest.BankKey}-{digest.ItemKey}?isaap={iSAAPcode}";
        }

        /// <summary>
        /// Gets the item digest's accessibility resources as a viewmodel
        /// </summary>
        /// <param name="itemDigest"></param>
        /// <returns>List of accessibility resource family</returns>
        private Task<List<AccessibilityResourceViewModel>> GetAccessibilityResourceViewModels(List<AccessibilityResource> accResources, string iSAAPCode)
        {
            return Task.Run(() =>  accResources.ToAccessibilityResourceViewModels(iSAAPCode));
        }

        /// <summary>
        /// Constructs a LocalAccessibilityViewModel using AccessiblityResourceViewModels
        /// </summary>
        /// <param name="accResourceViewModels"></param>
        private Task<LocalAccessibilityViewModel> GetLocalAccessibilityResourcesAsync(List<AccessibilityResourceViewModel> accResourceViewModels)
        {
            return Task.Run(() =>
            {
                LocalAccessibilityViewModel localAccViewModel = new LocalAccessibilityViewModel();

                List<AccessibilityResourceViewModel> nonApplicableResources = accResourceViewModels
                                         .Where(t => t.Disabled || t.AccessibilityListItems.TrueForAll(s => s.Disabled)).ToList();

                localAccViewModel.AccessibilityResourceViewModels = accResourceViewModels
                                        .Where(t => !nonApplicableResources.Contains(t))
                                        .ToList();

                localAccViewModel.NonApplicableAccessibilityResources = ConcatAccessibilityResources(nonApplicableResources);

                return localAccViewModel;
            });

        }

        /// <summary>
        /// Joins accessibility resource view model labels into 
        /// a comma-separated string
        /// </summary>
        /// <param name="accResourceViewModels"></param>
        private string ConcatAccessibilityResources(List<AccessibilityResourceViewModel> accResourceViewModels)
        {
            List<string> labels = accResourceViewModels.Select(t => t.Label).ToList();
            return string.Join(", ", labels);
        }

        /// <summary>
        /// Constructs an ItemViewModel with an ItemViewerService URL.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemViewModel.</returns>
        public Task<ItemViewModel> GetItemViewModelAsync(int bankKey, int itemKey)
        {
            return GetItemViewModelAsync(bankKey, itemKey, string.Empty);
        }

        /// <summary>
        /// Constructs an ItemViewModel with an ItemDigest, ItemViewerServiceURL,
        /// and accessibiltiyResources
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemViewModel.</returns>
        public async Task<ItemViewModel> GetItemViewModelAsync(int bankKey, int itemKey, string iSAAP)
        {
            ItemViewModel itemView = null;
            ItemDigest itemDigest = await GetItemDigestAsync(bankKey, itemKey);

            if (itemDigest != null)
            {
                itemView = new ItemViewModel();
                itemView.ItemDigest = itemDigest;
                itemView.ItemViewerServiceUrl = GetItemViewerUrl(itemView.ItemDigest, iSAAP);

                var accResourceVMs = await GetAccessibilityResourceViewModels(itemDigest?.AccessibilityResources, iSAAP);
                itemView.LocalAccessibilityViewModel = await GetLocalAccessibilityResourcesAsync(accResourceVMs);
            }

            return itemView;
        }

    }

}
