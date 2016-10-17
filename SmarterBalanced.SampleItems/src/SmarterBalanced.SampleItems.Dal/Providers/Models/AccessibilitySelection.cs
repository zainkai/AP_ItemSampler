using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilitySelection
    {
        public string Code { get; set; }

        public int Order { get; set; }

        public string Label { get; set; }

        public bool? Disabled { get; set; }
    }
}
