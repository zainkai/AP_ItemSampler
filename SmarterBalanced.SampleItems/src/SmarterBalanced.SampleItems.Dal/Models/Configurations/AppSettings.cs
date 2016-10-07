using Microsoft.Extensions.Configuration;
using SmarterBalanced.SampleItems.Dal.Models.Configurations;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Models.Configurations
{
    public class AppSettings
    {
        public SettingsConfig SettingsConfig { get; set; }
        public ExceptionMessages ExceptionMessages { get; set; }

        public AppSettings() { }

        public AppSettings(IConfigurationRoot configurations)
        {
            var appJsonRoot = configurations.GetSection("AppSettings");
            var settingsJson = appJsonRoot.GetSection("SettingsConfig");
            var exceptionJson = appJsonRoot.GetSection("ExceptionMessages");
            try
            {
                SettingsConfig = settingsJson.ConfigurationSectionToObject<SettingsConfig>();
                ExceptionMessages = exceptionJson.ConfigurationSectionToObject<ExceptionMessages>();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Invalid appsettings file.", e);
            }
        }
    }
}
