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

        private List<AccessibilityResourceViewModel> SetAccessibilityFromCookie(AccessibilityResourceViewModel[] cookiePreferences, List<AccessibilityResourceViewModel> defaultPreferences)
        {
            List<AccessibilityResourceViewModel> computedResources = new List<AccessibilityResourceViewModel>();

            //Use the defaults for any disabled accessibility resources
            computedResources = defaultPreferences.Where(r => r.Disabled).ToList();

            var disputedResources = defaultPreferences.Where(r => !r.Disabled);

            //Enabled resources
            foreach (AccessibilityResourceViewModel res in disputedResources)
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

        /// <summary>
        /// Converts a base64 encoded, serialized JSON string to an array of AccessibilityResourceViewModels
        /// </summary>
        /// <param name="cookieValue"></param>
        /// <returns></returns>
        private AccessibilityResourceViewModel[] DecodeCookie(string cookieValue)
        {
            try
            {
                byte[] data = Convert.FromBase64String(cookieValue);
                cookieValue = Encoding.UTF8.GetString(data);
                AccessibilityResourceViewModel[] cookiePreferences = JsonConvert.DeserializeObject<AccessibilityResourceViewModel[]>(cookieValue);
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
            AccessibilityResourceViewModel[] cookiePreferences = null;
            ItemDigest itemDigest = GetItemDigest(bankKey, itemKey);
            if (itemDigest == null)
            {
                return null;
            }

            if (iSAAPCodes.Length == 0)
            {
                cookiePreferences = DecodeCookie(cookieValue);
            }


            var accResources = itemDigest.AccessibilityResources.ToAccessibilityResourceViewModels(iSAAPCodes);
            if ((cookiePreferences != null) && (iSAAPCodes.Length == 0))
            {
                accResources = SetAccessibilityFromCookie(cookiePreferences, accResources);
            }

            var itemView = new ItemViewModel
            {
                ItemDigest = itemDigest,
                ItemViewerServiceUrl = GetItemViewerUrl(itemDigest),
                AccResourceVMs = accResources,
                AccessibilityCookieName = AppSettings.SettingsConfig.AccessibilityCookie,
            };

            return itemView;
        }

    }

}
