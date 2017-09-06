using System;
using System.Collections.Generic;
using System.Text;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    class ScoringGuideItemViewModel : ItemCardViewModel
    {
        public string Domain { get; }
        public string DepthOfKnowledge { get; }
        public string CommonCoreStandard { get; }

        public ScoringGuideItemViewModel(
            int bankKey,
            int itemKey,
            string title,
            GradeLevels grade,
            string gradeLabel,
            string subjectCode,
            string subjectLabel,
            string claimCode,
            string claimLabel,
            int targetHash,
            string targetShortName,
            string interactionTypeCode,
            string interactionTypeLabel,
            bool isPerformanceItem,
            bool brailleOnlyItem,
            string domain,
            string dok,
            string ccss): base(
                bankKey: bankKey,
                itemKey: itemKey,
                title: title,
                grade: grade,
                gradeLabel: gradeLabel,
                subjectCode: subjectCode,
                subjectLabel: subjectLabel,
                claimCode: claimCode,
                claimLabel: claimLabel,
                targetHash: targetHash,
                targetShortName: targetShortName,
                interactionTypeCode: interactionTypeCode,
                interactionTypeLabel: interactionTypeLabel,
                isPerformanceItem: isPerformanceItem,
                brailleOnlyItem: brailleOnlyItem)
        {
            Domain = domain;
            DepthOfKnowledge = dok;
            CommonCoreStandard = ccss;
        }
    }
}
