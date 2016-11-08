using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class InteractionFamily
    {
        public string Code { get; set; }
        public List<InteractionType> InteractionTypes { get; set; }
    }
}
