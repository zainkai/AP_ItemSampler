using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class Subject
    {
        public string Code { get; }
        public string Label { get; }
        public string ShortLabel { get; }
        public List<Claim> Claims { get; }
        public List<string> InteractionTypeCodes { get; }

        public Subject(string code, string label, string shortLabel, List<Claim> claims, List<string> interactionTypeCodes)
        {
            Code = code;
            Label = label;
            ShortLabel = shortLabel;
            Claims = claims;
            InteractionTypeCodes = interactionTypeCodes;
        }
    }
}
