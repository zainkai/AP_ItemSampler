using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmarterBalanced.SampleItems.Core.ScoreGuide.Models
{
    public class ScoreSearchParams
    {
        public GradeLevels Grades { get; }
        public IList<string> Subjects { get; }
        public IList<string> TechType { get; }
        public Boolean IsBraille { get; }
        public ScoreSearchParams(GradeLevels gradeLevels, string[] subjects, string[] techType, bool braille)
        {
            Grades = gradeLevels;
            Subjects = subjects;
            TechType = techType;
            IsBraille = braille;
        }
    }

}
