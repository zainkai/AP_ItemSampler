using SmarterBalanced.SampleItems.Dal.Models.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    public class AppSettings
    {
        public SettingsConfig SettingsConfig { get; set; }
        public ExceptionMessages ExceptionMessages {get;set;}
    }
}
