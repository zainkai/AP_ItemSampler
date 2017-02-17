using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class InteractionGroup
    {
        public ImmutableArray<InteractionFamily> InteractionFamilies { get; }
        public ImmutableArray<InteractionType> InteractionTypes { get; }

        public InteractionGroup(ImmutableArray<InteractionFamily> interactionFamilies, 
                            ImmutableArray<InteractionType> interactionTypes)
        {
            InteractionFamilies = interactionFamilies;
            InteractionTypes = interactionTypes;
        }
    }
}
