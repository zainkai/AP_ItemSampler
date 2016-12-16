using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class InteractionType
    {
        public string Code { get; }
        public string Label { get; }
        public int Order { get; }

        public InteractionType(string code, string label, int order)
        {
            Code = code;
            Label = label;
            Order = order;
        }
    }
}
