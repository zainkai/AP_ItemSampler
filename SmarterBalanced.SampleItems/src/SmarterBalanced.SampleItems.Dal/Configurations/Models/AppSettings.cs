using Microsoft.Extensions.Configuration;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;

namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
{
    public class AppSettings
    {
        public SettingsConfig SettingsConfig { get; set; }
        public ExceptionMessages ExceptionMessages { get; set; }
        public SbDiagnosticsSettings SbDiagnostics { get; set; }
        public SbBrailleSettings SbBraille { get; set; }
        public SbContentSettings SbContent { get; set; }

        public AppSettings() { }
    }
}
