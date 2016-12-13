using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class InteractionFamily
    {
        public string SubjectCode { get; }
        public ImmutableArray<string> InteractionTypeCodes { get; }

        public InteractionFamily(string subjectCode, ImmutableArray<string> interactionTypeCodes)
        {
            SubjectCode = subjectCode;
            InteractionTypeCodes = interactionTypeCodes;
        }
    }
}
