using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Translations
{
    public static class SampleItemTranslations
    {

        public static SampleItemViewModel ToSampleItemViewModel(this SampleItem sampleItem)
        {
            string claimTitle = (string.IsNullOrEmpty(sampleItem.Claim?.ClaimNumber)) ? string.Empty : $"Claim {sampleItem.Claim.ClaimNumber}";
            string title = $"{sampleItem.Subject?.ShortLabel} {sampleItem.Grade.ToDisplayString()} {claimTitle}";

            var vm = SampleItemViewModel.Create(
             bankKey: sampleItem.BankKey,
             itemKey: sampleItem.ItemKey,
             title: title,
             gradeLabel: sampleItem.Grade.ToDisplayString(),
             subjectCode: sampleItem.Subject?.Code,
             subjectLabel: sampleItem.Subject?.ShortLabel,
             claimCode: sampleItem.Claim?.Code,
             claimLabel: sampleItem.Claim?.Label,
             target: sampleItem.CoreStandards?.TargetIdLabel,
             interactionTypeCode: sampleItem.InteractionType?.Code,
             interactionTypeLabel: sampleItem.InteractionType?.Label,
             isPerformanceItem: sampleItem.IsPerformanceItem,
             aslSupported: sampleItem.AslSupported,
             stimulusKey: sampleItem.AssociatedStimulus,
             ccssDesc: sampleItem.CoreStandards?.CommonCoreStandardsDescription,
             targetDesc: sampleItem.CoreStandards?.TargetDescription);

            return vm;
        }
    }
}
