using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System.Collections.Immutable;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    /// <summary>
    /// Collection of known attributes of a Smarter Balanced item
    /// </summary>
    public sealed class SampleItem
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public string ItemType { get; set; }


        public GradeLevels Grade { get; set; }
        public Subject Subject { get; set; }
        public Claim Claim { get; set; }
        public ImmutableArray<Rubric> Rubrics { get; set; }
        public InteractionType InteractionType { get; set; }
        public ImmutableArray<AccessibilityResourceGroup> AccessibilityResourceGroups { get; set; }
        public string TargetAssessmentType { get; set; }
        public string SufficentEvidenceOfClaim { get; set; }
        public int? AssociatedStimulus { get; set; }
        public bool AslSupported { get; set; }
        public bool AllowCalculator { get; set; }
        public string DepthOfKnowledge { get; set; }
        public bool IsPerformanceItem { get; set; }

        public CoreStandards CoreStandards { get; set; }

        public SampleItem(
            int bankKey,
            int itemKey,
            string itemType,
            GradeLevels grade,
            Subject subject,
            Claim claim,
            ImmutableArray<Rubric> rubrics,
            InteractionType interactionType,
            ImmutableArray<AccessibilityResourceGroup> accessibilityResourceGroups,
            string targetAssessmentType,
            string sufficentEvidenceOfClaim,
            int? associatedStimulus,
            bool aslSupported,
            bool allowCalculator,
            string depthOfKnowledge,
            bool isPerformanceItem,
            CoreStandards coreStandards
            )
        {
            BankKey = bankKey;
            ItemKey = itemKey;
            ItemType = itemType;
            Grade = grade;
            Subject = subject;
            Claim = claim;
            Rubrics = rubrics;
            InteractionType = interactionType;
            AccessibilityResourceGroups = accessibilityResourceGroups;
            TargetAssessmentType = targetAssessmentType;
            SufficentEvidenceOfClaim = sufficentEvidenceOfClaim;
            AssociatedStimulus = associatedStimulus;
            AslSupported = aslSupported;
            AllowCalculator = allowCalculator;
            DepthOfKnowledge = depthOfKnowledge;
            IsPerformanceItem = isPerformanceItem;
            CoreStandards = coreStandards;
        }

        public static SampleItem Create(
            int bankKey = -1,
            int itemKey = -1,
            string itemType = "",
            GradeLevels grade = GradeLevels.NA,
            Subject subject = null,
            Claim claim = null,
            ImmutableArray<Rubric> rubrics = new ImmutableArray<Rubric>(),
            InteractionType interactionType = null,
            ImmutableArray<AccessibilityResourceGroup> accessibilityResourceGroups = new ImmutableArray<AccessibilityResourceGroup>(),
            string targetAssessmentType = "",
            string sufficentEvidenceOfClaim = "",
            int? associatedStimulus = -1,
            bool aslSupported = false,
            bool allowCalculator = false,
            string depthOfKnowledge = "",
            bool isPerformanceItem = false,
            CoreStandards coreStandards = null
            )
        {
            return new SampleItem(
                bankKey,
                itemKey,
                itemType,
                grade,
                subject,
                claim,
                rubrics,
                interactionType,
                accessibilityResourceGroups,
                targetAssessmentType,
                sufficentEvidenceOfClaim,
                associatedStimulus,
                aslSupported,
                allowCalculator,
                depthOfKnowledge,
                isPerformanceItem,
                coreStandards
            );
        }
    }    
}
