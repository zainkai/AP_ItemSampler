using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmarterBalanced.SampleItems.Core.ScoreGuide.Models
{
    public class ScoringGuideViewModel
    {
        public IList<InteractionType> InteractionTypes { get; set; }
        public IList<Subject> Subjects { get; set; }
    }
}
