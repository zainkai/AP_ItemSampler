using Microsoft.Extensions.Configuration;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;

namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
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
