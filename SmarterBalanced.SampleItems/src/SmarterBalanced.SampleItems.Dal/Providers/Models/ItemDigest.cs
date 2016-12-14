using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    /// <summary>
    /// Flattened digest of an ItemMetadata object and an ItemContents object
    /// </summary>
    public class ItemDigest : ItemPrimitive
    {
        public GradeLevels Grade { get; set; }
        public string DisplayGrade { get; set; }
        public string InteractionTypeLabel { get; set; }
        public List<AccessibilityResource> AccessibilityResources { get; set; }
        public string Title { get; set; }
        public Subject Subject { get; set; }
        public Claim Claim { get; set; }
    }
}
