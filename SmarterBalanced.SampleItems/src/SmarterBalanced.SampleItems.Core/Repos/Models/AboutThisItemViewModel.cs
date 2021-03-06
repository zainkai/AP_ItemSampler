﻿using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AboutThisItemViewModel
    {

        public ItemCardViewModel ItemCardViewModel { get; }
        public ImmutableArray<Rubric> Rubrics { get; }
        public string TargetDescription { get; }
        public string DepthOfKnowledge { get;}
        public string CommonCoreStandardsDescription { get; }

        public AboutThisItemViewModel(
            ImmutableArray<Rubric> rubrics, 
            ItemCardViewModel itemCard, 
            string targetDescription, 
            string depthOfKnowledge, 
            string commonCoreStandardsDescription)
        {
            ItemCardViewModel = itemCard;
            Rubrics = rubrics;
            TargetDescription = targetDescription;
            DepthOfKnowledge = depthOfKnowledge;
            CommonCoreStandardsDescription = commonCoreStandardsDescription;
        }

    }
}
