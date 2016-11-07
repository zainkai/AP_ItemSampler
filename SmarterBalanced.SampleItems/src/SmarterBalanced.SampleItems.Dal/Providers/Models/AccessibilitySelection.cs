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

        public bool Disabled { get; set; }

        public AccessibilitySelection Clone()
        {
            return new AccessibilitySelection
            {
                Code = Code,
                Order = Order,
                Label = Label,
                Disabled = Disabled
            };
        }

        public AccessibilitySelection CloneWithDisabled(bool disabled)
        {
            var selection = this.Clone();
            selection.Disabled = disabled;
            return selection;
        }
    }
}
