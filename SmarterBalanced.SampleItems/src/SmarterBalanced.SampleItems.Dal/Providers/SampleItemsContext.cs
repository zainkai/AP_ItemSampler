using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Xml;

namespace SmarterBalanced.SampleItems.Dal.Providers
{
    public sealed class SampleItemsContext
    {
        public IList<ItemDigest> ItemDigests { get; set; }
        public IList<InteractionType> InteractionTypes { get; set; }
        public IList<ClaimSubject> ClaimSubjects { get; set; }
        public IList<AccessibilityResource> GlobalAccessibilityResources { get; set; }
        public IList<AccessibilityResourceFamily> AccessibilityResourceFamilies { get; set; }
        public AppSettings AppSettings { get; set; }
    }
}
