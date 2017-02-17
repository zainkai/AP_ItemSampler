using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class CoreStandards
    {
        public string TargetDescription { get; }
        public string TargetId { get; }
        public string TargetIdLabel { get; }
        public string CommonCoreStandardsId { get; }
        public string CommonCoreStandardsDescription { get; }

        public CoreStandards(
            string targetDescription,
            string targetId,
            string targetIdLabel,
            string commonCoreStandardsId,
            string commonCoreStandardsDescription)
        {
            TargetDescription = targetDescription;
            TargetId = targetId;
            TargetIdLabel = targetIdLabel;
            CommonCoreStandardsId = commonCoreStandardsId;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
        }

        public static CoreStandards Create(
          string targetDescription = "",
          string targetId = "",
          string targetIdLabel = "",
          string commonCoreStandardsId = "",
          string commonCoreStandardsDescription = "")
        {
            return new CoreStandards(
                targetDescription: targetDescription,
                targetId: targetId,
                targetIdLabel:  targetIdLabel,
                commonCoreStandardsId: commonCoreStandardsId,
                commonCoreStandardsDescription: commonCoreStandardsDescription);
        }

    }
}
