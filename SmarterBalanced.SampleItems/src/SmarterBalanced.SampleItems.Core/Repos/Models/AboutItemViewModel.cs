using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AboutItemViewModel
    {
        public int ItemKey { get; }
        public string CommonCoreStandardsId { get; }
        public string TargetId { get; }
        public GradeLevels Grade { get;}
        public ImmutableArray<Rubric> Rubrics { get; }

        public AboutItemViewModel(
            int itemKey,
            string commonCoreStandardsId,
            string targetId,
            GradeLevels grade,
            ImmutableArray<Rubric> rubrics)
        {
            ItemKey = itemKey;
            CommonCoreStandardsId = commonCoreStandardsId;
            TargetId = targetId;
            Grade = grade;
            Rubrics = rubrics;
        }
    }
}
