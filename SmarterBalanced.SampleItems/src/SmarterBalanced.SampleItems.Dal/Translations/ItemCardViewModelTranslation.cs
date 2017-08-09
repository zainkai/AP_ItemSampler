using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Translations
{
    public static class ItemCardViewModelTranslation
    {
        public static ItemCardViewModel ToItemCardViewModel(this SampleItem sampleItem)
        {
            string claimTitle = (string.IsNullOrEmpty(sampleItem.Claim?.ClaimNumber)) ? string.Empty : $"Claim {sampleItem.Claim.ClaimNumber}";
            string title = $"{sampleItem.Subject?.ShortLabel} {sampleItem.Grade.ToDisplayString()} {claimTitle}";

            var card = ItemCardViewModel.Create(
                bankKey: sampleItem.BankKey,
                itemKey: sampleItem.ItemKey,
                title: title,
                grade: sampleItem.Grade,
                gradeLabel: sampleItem.Grade.ToDisplayString(),
                subjectCode: sampleItem.Subject?.Code,
                subjectLabel: sampleItem.Subject?.ShortLabel,
                claimCode: sampleItem.Claim?.Code,
                claimLabel: sampleItem.Claim?.Label,
                targetHash: sampleItem.CoreStandards?.Target?.GetHashCode() ?? 0,
                targetShortName: sampleItem.CoreStandards?.Target?.Name,
                interactionTypeCode: sampleItem.InteractionType?.Code,
                interactionTypeLabel: sampleItem.InteractionType?.Label,
                isPerformanceItem: sampleItem.IsPerformanceItem,
                brailleOnlyitem: sampleItem.BrailleOnlyItem);

            return card;
        }
    }
}
