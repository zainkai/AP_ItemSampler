using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class MoreLikeThisColumn
    {
        public string Label { get; }
        public ImmutableArray<ItemCardViewModel> ItemCards { get; }

        public MoreLikeThisColumn(string label, ImmutableArray<ItemCardViewModel> itemCards)
        {
            this.Label = label;
            this.ItemCards = itemCards;
        }

    }
}
