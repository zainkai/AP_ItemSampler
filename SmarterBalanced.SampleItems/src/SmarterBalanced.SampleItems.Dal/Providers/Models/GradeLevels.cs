using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    [Flags]
    public enum GradeLevels
    {
        NA = 0,
        Grade3 = 1 << 0,
        Grade4 = 1 << 1,
        Grade5 = 1 << 2,
        Grade6 = 1 << 3,
        Grade7 = 1 << 4,
        Grade8 = 1 << 5,
        Grade9 = 1 << 6,
        Grade10 = 1 << 7,
        Grade11 = 1 << 8,
        Grade12 = 1 << 9,
        Elementary = Grade3 | Grade4 | Grade5,
        Middle = Grade6 | Grade7 | Grade8,
        High = Grade9 | Grade10 | Grade11 | Grade12,
        All = Elementary | Middle | High
    }

    public static class GradeLevelsUtils
    {
        public static GradeLevels FromString(string s)
        {
            switch (s)
            {
                case "3": return GradeLevels.Grade3;
                case "4": return GradeLevels.Grade4;
                case "5": return GradeLevels.Grade5;
                case "6": return GradeLevels.Grade6;
                case "7": return GradeLevels.Grade7;
                case "8": return GradeLevels.Grade8;
                case "9": return GradeLevels.Grade9;
                case "10": return GradeLevels.Grade10;
                case "11": return GradeLevels.Grade11;
                case "12": return GradeLevels.Grade12;
                case "NA": return GradeLevels.NA;
                default: throw new ArgumentException($"String \"{s}\" is not a valid grade level.");
            }
        }

        public static bool Contains(GradeLevels haystack, GradeLevels needle)
        {
            return (haystack & needle) != GradeLevels.NA;
        }
    }
}
