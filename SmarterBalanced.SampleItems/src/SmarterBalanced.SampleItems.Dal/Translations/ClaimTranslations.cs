using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class ClaimTranslations
    {
        public static ImmutableArray<Claim> ToClaims(this IEnumerable<XElement> claimElements)
        {
            var claims = claimElements
                .Select(ToClaim)
                .ToImmutableArray();

            return claims;
        }

        public static Claim ToClaim(this XElement claimElement)
        {
            var claim = new Claim(
                code: (string)claimElement.Element("Code"),
                label: (string)claimElement.Element("Label"),
                claimNumber: (string)claimElement.Element("ClaimNumber"));

            return claim;
        }

    }
}
