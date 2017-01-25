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

        public static AccessibilitySelection Create(
            string code = "",
            string label = "",
            int order = -1,
            bool disabled = false)
        {
            var sel = new AccessibilitySelection(
                code: code,
                order: order,
                disabled: disabled,
                label: label);

            return sel;
        }
        
        public AccessibilitySelection WithDisabled(bool disabled)
        {
            var newSelection = new AccessibilitySelection(
                code: Code,
                label: Label,
                order: Order,
                disabled: disabled);

            return newSelection;
        }
    }

}
