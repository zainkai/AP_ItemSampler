using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;

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
        public AppSettings GetSettings()
        {
            return context.AppSettings();
        }

        /// <summary>
        /// Get all ItemDigests with default order (BankKey, then ItemKey).
        /// </summary>
        /// <returns>
        /// An IEnumerable of ItemDigests
        /// </returns>
        public IEnumerable<ItemDigest> GetItemDigests()
        {
            return context.ItemDigests.OrderBy(t => t.BankKey)
                                        .ThenBy(t => t.ItemKey);
        }

        /// <summary>
        /// Retreives the single specified ItemDigest.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().SingleOrDefault(predicate);
        }

        /// <summary>
        /// Retrieves an ItemDigest matching the given bankKey and itemKey.
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemDigest object.</returns>
        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return GetItemDigest(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
        /// <param name="digest"></param>
        /// <returns>a string URL.</returns>
        private string GetItemViewerUrl(ItemDigest digest, string iSSAPcode)
        {
            if (digest == null)
            {
                return string.Empty;
            }

            string baseUrl = context.AppSettings().SettingsConfig.ItemViewerServiceURL;
            return $"{baseUrl}/item/{digest.BankKey}-{digest.ItemKey}?isaap={iSSAPcode}";
        }

        /// <summary>
        /// Gets the item digest's accessibility resources as a viewmodel
        /// </summary>
        /// <param name="itemDigest"></param>
        /// <returns>List of accessibility resource family</returns>
        private List<AccessibilityResourceViewModel> GetAccessibilityResourceViewModels(List<AccessibilityResource> accResources, string iSSAPCode)
        {
            return accResources.ToAccessibilityResourceViewModels(iSSAPCode);
        }

        /// <summary>
        /// Constructs a LocalAccessibilityViewModel using AccessiblityResourceViewModels
        /// </summary>
        /// <param name="accResourceViewModels"></param>
        private LocalAccessibilityViewModel GetLocalAccessibilityResources(List<AccessibilityResourceViewModel> accResourceViewModels)
        {
            LocalAccessibilityViewModel localAccViewModel = new LocalAccessibilityViewModel();

            List<AccessibilityResourceViewModel> nonApplicableResources = accResourceViewModels
                .Where(t => t.Disabled || t.AccessibilityListItems.TrueForAll(s => s.Disabled)).ToList();

            localAccViewModel.AccessibilityResourceViewModels = accResourceViewModels
                .Where(t => !nonApplicableResources.Contains(t))
                .ToList();

            localAccViewModel.NonApplicableAccessibilityResources = ConcatAccessibilityResources(nonApplicableResources);

            return localAccViewModel;
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
        public ItemViewModel GetItemViewModel(int bankKey, int itemKey)
        {
            return GetItemViewModel(bankKey, itemKey, string.Empty);
        }

        /// <summary>
        /// Constructs an ItemViewModel with an ItemDigest, ItemViewerServiceURL,
        /// and accessibiltiyResources
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>an ItemViewModel.</returns>
        public ItemViewModel GetItemViewModel(int bankKey, int itemKey, string iSSAP)
        {
            ItemViewModel itemView = null;
            ItemDigest itemDigest = GetItemDigest(bankKey, itemKey);

            if (itemDigest != null)
            {
                itemView = new ItemViewModel();
                itemView.ItemDigest = itemDigest;
                itemView.ItemViewerServiceUrl = GetItemViewerUrl(itemView.ItemDigest, iSSAP);

                var accResourceVMs = GetAccessibilityResourceViewModels(itemDigest?.AccessibilityResources, iSSAP);
                itemView.LocalAccessibilityViewModel = GetLocalAccessibilityResources(accResourceVMs);
            }

            return itemView;
        }
        
        /// <summary>
        /// Updates a LocalAccessibilityViewModel with the given ISSAP code.
        /// </summary>
        /// <param name="localAccViewModel"></param>
        /// <param name="iSSAP"></param>
        /// <returns>a LocalAccessibilityViewModel.</returns>
        public LocalAccessibilityViewModel UpdateAccessibility(LocalAccessibilityViewModel localAccViewModel, string iSSAP)
        {
            localAccViewModel.AccessibilityResourceViewModels = AccessibilityTranslations
                .UpdateAccessibilityResourceViewModels(
                localAccViewModel?.AccessibilityResourceViewModels, iSSAP);

            return localAccViewModel;
        }

    }

}
