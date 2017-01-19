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

        public AppSettings AppSettings
        {
            get
            {
                return context.AppSettings;
            }
        }

        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return context.ItemDigests.SingleOrDefault(item => item.BankKey == bankKey && item.ItemKey == itemKey);
        }

        /// <summary>
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
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
        /// Constructs an itemviewerservice URL to access the 
        /// item corresponding to the given ItemDigest.
        /// </summary>
        private string GetItemViewerUrl(ItemDigest digest)
        {
            if (digest == null)
            {
                return string.Empty;
            }

            string baseUrl = context.AppSettings.SettingsConfig.ItemViewerServiceURL;
            return $"{baseUrl}/item/{digest.BankKey}-{digest.ItemKey}";
        }

        private List<AccessibilityResource> SetResourceValuesFromCookie(ImmutableArray<AccessibilityResource> cookiePreferences, ImmutableArray<AccessibilityResource> defaultPreferences)
        {
            List<AccessibilityResource> computedResources = new List<AccessibilityResource>();

            //Use the defaults for any disabled accessibility resources
            computedResources = defaultPreferences.Where(r => r.Disabled).ToList();

            var disputedResources = defaultPreferences.Where(r => !r.Disabled);

            //Enabled resources
            foreach (AccessibilityResource res in disputedResources)
            {
                try
                {
                    var userPref = cookiePreferences.Where(p => p.Label == res.Label).SingleOrDefault();
                    var defaultSelDisabled = res.Selections.Where(s => s.Code == userPref.SelectedCode).SingleOrDefault();
                    var selected = userPref.SelectedCode;
                    if (!defaultSelDisabled.Disabled)
                    {
                        res.SelectedCode = userPref.SelectedCode;
                    }
                }
                catch (Exception e) when (
                    e is ArgumentNullException ||
                    e is InvalidOperationException ||
                    e is NullReferenceException)
                {
                    //There was a mismatch between the user's supplied preferences and the allowed values, 
                    //or there was duplidate data
                    //Use the default which is already set
                    logger.LogInformation(e.ToString());
                }

                computedResources.Add(res);
            }

            return computedResources;
        }

        private List<AccessibilityResourceGroup> SetAccessibilityFromCookie(AccessibilityResourceGroup[] cookiePreferences, ImmutableArray<AccessibilityResourceGroup> defaultPreferences)
        {
            List<AccessibilityResourceGroup> resourceGroups = new List<AccessibilityResourceGroup>();
            foreach (AccessibilityResourceGroup group in defaultPreferences)
            {
                ImmutableArray<AccessibilityResource> cookieResources;
                ImmutableArray<AccessibilityResource> computedResources;
                try
                {
                    cookieResources = cookiePreferences.Where(g => g.Order == group.Order).First().AccessibilityResources;
                    computedResources = SetResourceValuesFromCookie(group.AccessibilityResources, cookieResources)
                        .OrderBy(r => r.Order)
                        .ToImmutableArray();
                }
                catch(Exception e)
                {
                    //Fall back to the defaults if there are no user preferences for the group
                    if(e is ArgumentNullException || e is InvalidOperationException)
                    {
                        logger.LogDebug($"Cookie does not contain user accessibility preferences for {group.Label} group");
                    }
                    else
                    {
                        throw;
                    }
                    computedResources = group.AccessibilityResources;
                }

                resourceGroups.Add(new AccessibilityResourceGroup(
                    label: group.Label,
                    order: group.Order,
                    accessibilityResources: computedResources
                    ));
            }
            return resourceGroups;
        }

        /// <summary>
        /// Converts a base64 encoded, serialized JSON string to an array of AccessibilityResourceViewModels
        /// </summary>
        /// <param name="cookieValue"></param>
        /// <returns></returns>
        private AccessibilityResourceGroup[] DecodeCookie(string cookieValue)
        {
            try
            {
                byte[] data = Convert.FromBase64String(cookieValue);
                cookieValue = Encoding.UTF8.GetString(data);
                AccessibilityResourceGroup[] cookiePreferences = JsonConvert.DeserializeObject<AccessibilityResourceGroup[]>(cookieValue);
                return cookiePreferences;
            }
            catch (Exception e)
            {
                logger.LogInformation("Unable to deserialize user accessibility options from cookie. Reason: "
                    + e.Message);
                return null;
            }
        }



        /// <returns>
        /// An ItemViewModel instance, or null if no item exists with
        /// the given combination of bankKey and itemKey.
        /// </returns>
        public ItemViewModel GetItemViewModel(int bankKey, int itemKey, string[] iSAAPCodes,
            string cookieValue)
        {
            AccessibilityResourceGroup[] cookiePreferences = null;
            ItemDigest itemDigest = GetItemDigest(bankKey, itemKey);
            if (itemDigest == null)
            {
                return null;
            }

            AboutItemViewModel aboutItem = itemDigest.ToAboutItemViewModel();

            if (iSAAPCodes.Length == 0)
            {
                cookiePreferences = DecodeCookie(cookieValue);
            }

            var accResources = itemDigest.AccessibilityResourceGroups.SetIsaap(iSAAPCodes);
            if ((cookiePreferences != null) && (iSAAPCodes.Length == 0))
            {
                accResources = SetAccessibilityFromCookie(cookiePreferences, accResources).ToImmutableArray();
            }

            return new ItemViewModel(
                            itemViewerServiceUrl: GetItemViewerUrl(itemDigest),
                            accessibilityCookieName: AppSettings.SettingsConfig.AccessibilityCookie,
                            aboutItemVM: aboutItem,
                            accResourceGroups: accResources
                        );
        }

    }

}
