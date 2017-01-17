using Microsoft.Extensions.Configuration;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;

namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
{
    public class AppSettings
    {
        public SettingsConfig SettingsConfig { get; set; }
        public ExceptionMessages ExceptionMessages { get; set; }

        public RubricPlaceHolderText RubricPlaceHolderText {get;set;}
        public AppSettings() { }
    }
}
