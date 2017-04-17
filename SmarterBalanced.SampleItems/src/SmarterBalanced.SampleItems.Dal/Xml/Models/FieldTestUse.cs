using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class FieldTestUse 
    {
        public int? QuestionNumber { get; set; }
        public string Code { get; set; }

        public string Section { get; set; }

        public int? CodeYear { get; set; }

        private static FieldTestUse Create(Match match)
        {
            if(match.Groups.Count != 6)
            {
                return null;
            }
            int temp;
            int? codeYear = int.TryParse(match.Groups[4].Value, out temp) ? temp : (int?)null;
            int? questionNumber = int.TryParse(match.Groups[5].Value, out temp) ? temp : (int?)null;

            FieldTestUse fieldTestUse = new FieldTestUse
            {
                Code = match.Groups[3].Value,
                QuestionNumber = questionNumber,
                Section = match.Groups[2].Value,
                CodeYear = codeYear
            };

            return fieldTestUse;

        }

        public static FieldTestUse Create(ItemMetadataAttribute attribute, string subjectCode)
        {
            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                return null;
            }

            MatchCollection matches = Regex.Matches(input: attribute.Value, pattern: @"(S(\d)..)*(SP{1}(\d{2}))\s\((\d)\)");
            if (matches.Count == 0)
            {
                return null;
            }

            List<FieldTestUse> fields = new List<FieldTestUse>();
            foreach (Match match in matches)
            {
                var fieldUse = Create(match);
                if(fieldUse != null)
                {
                    fields.Add(Create(match));
                }
            }

            return fields.FirstOrDefault();
        }
    }
}
