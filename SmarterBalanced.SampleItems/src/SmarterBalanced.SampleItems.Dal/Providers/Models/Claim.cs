using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class Claim
    {
        /// <summary>
        /// A claim identifier within a subject. e.g. ELA2
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// A claim identifier shared across subjects. e.g. Claim2
        /// </summary>
        public string ClaimNumber { get; }

        /// <summary>
        /// The user facing name of the claim. e.g. Problem Solving and Modeling
        /// </summary>
        public string Label { get; }

        public Claim(string code, string claimNumber, string label)
        {
            Code = code;
            ClaimNumber = claimNumber;
            Label = label;
        }
    }
}
