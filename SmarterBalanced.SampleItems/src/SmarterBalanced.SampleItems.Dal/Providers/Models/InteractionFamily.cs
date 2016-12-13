using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class InteractionFamily
    {
        public string SubjectCode { get; }
        public List<string> InteractionTypeCodes { get; }

        public InteractionFamily(string subjectCode, List<string> interactionTypeCodes)
        {
            SubjectCode = subjectCode;
            InteractionTypeCodes = interactionTypeCodes;
        }
    }
}
