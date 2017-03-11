﻿using SmarterBalanced.SampleItems.Dal.Xml.Models;
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
        public int BankKey { get; }
        public int ItemKey { get; }
        public string ItemType { get; }


        public GradeLevels Grade { get; }
        public Subject Subject { get; }
        public Claim Claim { get; }
        public ImmutableArray<Rubric> Rubrics { get; }
        public InteractionType InteractionType { get; }
        public ImmutableArray<AccessibilityResourceGroup> AccessibilityResourceGroups { get; }
        public string TargetAssessmentType { get; }
        public string SufficentEvidenceOfClaim { get; }
        public int? AssociatedStimulus { get; }
        public bool AslSupported { get; }
        public bool AllowCalculator { get; }
        public string DepthOfKnowledge { get; }
        public bool IsPerformanceItem { get; }
        public FieldTestUse FieldTestUse { get;}
        public CoreStandards CoreStandards { get; }
        public string InteractionTypeSubCat{ get; }

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
            CoreStandards coreStandards,
            FieldTestUse fieldTestUse,
            string interactionTypeSubCat
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
            FieldTestUse = fieldTestUse;
            InteractionTypeSubCat = interactionTypeSubCat;
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
            CoreStandards coreStandards = null,
            FieldTestUse fieldTestUse = null,
            string interactionTypeSubCat = "")
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
                coreStandards,
                fieldTestUse,
                interactionTypeSubCat);
        }

        public override string ToString()
        {
            return $"{BankKey}-{ItemKey}";
        }
    }    
}
