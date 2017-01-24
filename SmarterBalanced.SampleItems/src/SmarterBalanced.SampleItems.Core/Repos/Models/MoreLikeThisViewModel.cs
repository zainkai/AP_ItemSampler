using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class MoreLikeThisViewModel
    {
        public MoreLikeThisColumn GradeBelowItems { get; }
        public MoreLikeThisColumn SameGradeItems { get; }
        public MoreLikeThisColumn GradeAboveItems { get; }

        public MoreLikeThisViewModel(
            MoreLikeThisColumn gradeBelowItems,
            MoreLikeThisColumn sameGradeItems,
            MoreLikeThisColumn gradeAboveItems)
        {
            GradeAboveItems = gradeAboveItems;
            SameGradeItems = sameGradeItems;
            GradeBelowItems = gradeBelowItems;
        }

    }
}
