using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class Subject
    {
        public string Code { get; }
        public string Label { get; }
        public string ShortLabel { get; }
        public ImmutableArray<Claim> Claims { get; }
        public ImmutableArray<string> InteractionTypeCodes { get; }

        public Subject(string code, string label, string shortLabel, ImmutableArray<Claim> claims, ImmutableArray<string> interactionTypeCodes)
        {
            Code = code;
            Label = label;
            ShortLabel = shortLabel;
            Claims = claims;
            InteractionTypeCodes = interactionTypeCodes;
        }
    }
}
