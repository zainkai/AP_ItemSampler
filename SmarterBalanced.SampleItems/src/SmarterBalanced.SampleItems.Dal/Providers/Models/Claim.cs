using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class Claim
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

        public static Claim Create(string code = "", string claimNumber = "", string label = "")
        {
            return new Claim(
                 code: code,
                 claimNumber: claimNumber,
                 label: label);
        }

        public static Claim Create(XElement element)
        {
            var claim = new Claim(
                code: (string)element.Element("Code"),
                label: (string)element.Element("Label"),
                claimNumber: (string)element.Element("ClaimNumber"));

            return claim;
        }
    }
}
