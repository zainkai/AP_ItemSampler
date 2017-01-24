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
        public ImmutableArray<ItemCardViewModel> GradeBelowItems { get; }
        public ImmutableArray<ItemCardViewModel> SameGradeItems { get; }
        public ImmutableArray<ItemCardViewModel> GradeAboveItems { get; }

        public MoreLikeThisViewModel(
            ImmutableArray<ItemCardViewModel> gradeBelowItems,
            ImmutableArray<ItemCardViewModel> sameGradeItems,
            ImmutableArray<ItemCardViewModel> gradeAboveItems)
        {
            GradeAboveItems = gradeAboveItems;
            SameGradeItems = sameGradeItems;
            GradeBelowItems = gradeBelowItems;
        }

    }
}
