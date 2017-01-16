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
        public string CommonCoreStandardsId { get;}
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
    }
}
