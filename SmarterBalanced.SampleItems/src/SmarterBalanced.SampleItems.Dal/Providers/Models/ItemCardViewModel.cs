using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public sealed class ItemCardViewModel
    {
        public int BankKey { get; }
        public int ItemKey { get; }
        public string Title { get; }
        public GradeLevels Grade { get; }
        public string GradeLabel { get; }
        public string SubjectCode { get; }
        public string SubjectLabel { get; }
        public string ClaimCode { get; }
        public string ClaimLabel { get; }
        public string Target { get; }
        public string InteractionTypeCode { get; }
        public string InteractionTypeLabel { get; }
        public string CommonCoreStandardsId { get; }
        public ItemCardViewModel(
            int bankKey,
            int itemKey,
            string title,
            GradeLevels grade,
            string gradeLabel,
            string subjectCode,
            string subjectLabel,
            string claimCode,
            string claimLabel,
            string target,
            string interactionTypeCode,
            string interactionTypeLabel,
            string commonCoreStandardsId)
        {
            BankKey = bankKey;
            ItemKey = itemKey;
            Title = title;
            Grade = grade;
            GradeLabel = gradeLabel;
            SubjectCode = subjectCode;
            SubjectLabel = subjectLabel;
            ClaimCode = claimCode;
            ClaimLabel = claimLabel;
            Target = target;
            InteractionTypeCode = interactionTypeCode;
            InteractionTypeLabel = interactionTypeLabel;
            CommonCoreStandardsId = commonCoreStandardsId;
        }

        /// <summary>
        /// Used for testing so that it's not necessary to specify all parameters.
        /// </summary>
        public static ItemCardViewModel Create(
           int bankKey = -1,
           int itemKey = -1,
           string title = "",
           GradeLevels grade = GradeLevels.NA,
           string gradeLabel = "",
           string subjectCode = "",
           string subjectLabel = "",
           string claimCode = "",
           string claimLabel = "",
           string target = "",
           string interactionTypeCode = "",
           string interactionTypeLabel = "",
           string commonCoreStandardsId = "")
        {
            return new ItemCardViewModel(
                bankKey: bankKey,
                itemKey: itemKey,
                title: title,
                grade: grade,
                gradeLabel: gradeLabel,
                subjectCode: subjectCode,
                subjectLabel: subjectLabel,
                claimCode: claimCode,
                claimLabel: claimLabel,
                target: target,
                interactionTypeCode: interactionTypeCode,
                interactionTypeLabel: interactionTypeLabel,
                commonCoreStandardsId: commonCoreStandardsId);
        }

    }

    public class MoreLikeThisComparer : IComparer<ItemCardViewModel>
    {
        private readonly string subjectCode;
        private readonly string claimCode;

        public MoreLikeThisComparer(string subjectCode, string claimCode)
        {
            this.subjectCode = subjectCode;
            this.claimCode = claimCode;
        }

        private int Weight(ItemCardViewModel itemCardVM)
        {
            int weight = 2;
            if (itemCardVM.SubjectCode == subjectCode)
                weight--;

            if (itemCardVM.ClaimCode == claimCode)
                weight--;

            return weight;
        }

        /// <summary>
        /// Compares ItemCardViewModel by subject and claim similarity
        /// </summary>
        /// <remarks>
        /// positive return value: x is bigger (x - y)
        /// negative return value: y is bigger
        /// "bigger" means it will appear later when sorted in ascending order
        /// </remarks>
        public int Compare(ItemCardViewModel x, ItemCardViewModel y)
        {
            int weightDiff = Weight(x) - Weight(y);

            return weightDiff;
        }

    }

}
