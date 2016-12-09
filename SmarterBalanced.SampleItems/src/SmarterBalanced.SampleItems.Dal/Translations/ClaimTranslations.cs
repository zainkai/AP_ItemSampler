using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class ClaimTranslations
    {
        /// <summary>
        /// Translates Claims xml doc into a list of ClaimSubjects.
        /// </summary>
        /// <param name="xdoc"></param>
        /// <returns>A list of ClaimSubjects.</returns>
        public static List<ClaimSubject> ToClaimSubjects(this XDocument xdoc)
        {
            List<ClaimSubject> claimSubjects = xdoc
                .Element("Claims")
                .Element("Subjects")
                .Elements("Subject").ToClaimSubjects();

            return claimSubjects;
        }

        /// <summary>
        /// Translates an IEnumerable of Subject XElements into a list of ClaimSubjects.
        /// </summary>
        /// <param name="subjects"></param>
        /// <returns>A list of ClaimSubjects.</returns>
        public static List<ClaimSubject> ToClaimSubjects(this IEnumerable<XElement> subjects)
        {
            List<ClaimSubject> claimSubjects = subjects
                .Select(s => new ClaimSubject
                {
                    Code = (string)s.Element("Code"),
                    Claims = s.Elements("Claim").ToClaims()
                }).ToList();

            return claimSubjects;
        }

        /// <summary>
        /// Translates an IEnumerable of Claim XElements into a list of Claims.
        /// </summary>
        /// <param name="claimElements"></param>
        /// <returns>A List of Claims.</returns>
        public static List<Claim> ToClaims(this IEnumerable<XElement> claimElements)
        {
            List<Claim> claims = claimElements
                .Select(c => new Claim
                {
                    Code = (string)c.Element("Code"),
                    Label = (string)c.Element("Label"),
                    ClaimNumber = (string)c.Element("ClaimNumber")
                }).ToList();

            return claims;
        }

    }
}
