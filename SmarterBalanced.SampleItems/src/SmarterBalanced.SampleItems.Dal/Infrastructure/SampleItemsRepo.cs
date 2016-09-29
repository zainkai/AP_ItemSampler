using SmarterBalanced.SampleItems.Dal.Context;
using SmarterBalanced.SampleItems.Dal.Interfaces;
using SmarterBalanced.SampleItems.Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SmarterBalanced.SampleItems.Dal.Models.Configurations;
using SmarterBalanced.SampleItems.Dal.Translations;

namespace SmarterBalanced.SampleItems.Dal.Infrastructure
{
    public class SampleItemsRepo : ISampleItemsRepo
    {
        private ISampleItemsContext sampleItemsContext;
        private static SampleItemsRepo sampleItemsSingletonInstance;
        private static AppSettings Settings { get; set; }

        /// <summary>
        /// Adds the appsettings configuration into AppSettings class
        /// </summary>
        /// <param name="configurations"></param>
        /// TODO: Throw custom exception and add error logging
        public static void BuildConfiguration(IConfigurationRoot configurations)
        {
            var appJsonRoot = configurations.GetSection("AppSettings");
            var settingsJson = appJsonRoot.GetSection("SettingsConfig");
            var exceptionJson = appJsonRoot.GetSection("ExceptionMessages");
            var appSettings = new AppSettings();
            try
            {
                appSettings.SettingsConfig = settingsJson.ConfigurationSectionToObject<SettingsConfig>();
                appSettings.ExceptionMessages = exceptionJson.ConfigurationSectionToObject<ExceptionMessages>();
            }
            catch(Exception e)
            {
                throw new Exception("Invalid appsettings file");
            }

            Settings = appSettings;
        }

        private SampleItemsRepo()
        {
            sampleItemsContext = new SampleItemsContext(GetSettings());
        }
        
        public static SampleItemsRepo Default
        {
            get
            {
                if(sampleItemsSingletonInstance == null)
                {
                    sampleItemsSingletonInstance = new SampleItemsRepo();
                }

                return sampleItemsSingletonInstance;
            }
        }

        /// <summary>
        /// Get all ItemDigests with default order (BankKey, then ItemKey).
        /// </summary>
        /// <returns>
        /// An IEnumerable of ItemDigests
        /// </returns>
        public IEnumerable<ItemDigest> GetItemDigests()
        {
            return sampleItemsContext.ItemDigests.OrderBy(t => t.BankKey)
                                        .ThenBy(t => t.ItemKey);
        }

        /// <summary>
        /// Get all ItemDigests matching the given predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>An IEnumerable of ItemDigests</returns>
        public IEnumerable<ItemDigest> GetItemDigests(Func<ItemDigest, bool> predicate)
        {
            return GetItemDigests().Where(predicate);
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
        /// Get ItemDigest matching the specified identifier keys
        /// </summary>
        /// <param name="bankKey"></param>
        /// <param name="itemKey"></param>
        /// <returns>ItemDigest</returns>
        public ItemDigest GetItemDigest(int bankKey, int itemKey)
        {
            return GetItemDigest(t => t.BankKey == bankKey && t.ItemKey == itemKey);
        }

        public AppSettings GetSettings()
        {
            return Settings;
        }
    }
}
