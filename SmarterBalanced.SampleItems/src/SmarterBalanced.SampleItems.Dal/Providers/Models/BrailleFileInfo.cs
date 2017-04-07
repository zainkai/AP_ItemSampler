using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class BrailleFileInfo
    {
        public int ItemKey { get; }
        public string Subject { get; }
        public GradeLevels Grade { get; }
        public string BrailleType { get; }
        public BrailleFileInfo(int itemKey, string subject, GradeLevels grade, string brailleType)
        {
            ItemKey = itemKey;
            Subject = subject;
            Grade = grade;
            BrailleType = brailleType;
        }
    }
}
