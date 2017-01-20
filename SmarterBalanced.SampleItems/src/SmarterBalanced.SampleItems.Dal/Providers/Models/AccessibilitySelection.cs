using System;
using System.Collections.Generic;
using System.Linq;


namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilitySelection
    {
        public string Code { get; }
        public string Label { get; }
        public int Order { get; }
        public bool Disabled { get; }

        public AccessibilitySelection(
            string code,
            string label,
            int order,
            bool disabled)
        {
            Code = code;
            Label = label;
            Order = order;
            Disabled = disabled;
        }

    }

}
