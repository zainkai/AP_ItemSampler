using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Xml.Models
{
    public class FieldTestUse
    {
        public int? QuestionNumber { get; set; }
        public string Code { get; set; }

        public string Section { get; set; }

        public static FieldTestUse Create(ItemMetadataAttribute attribute, string subjectCode)
        {
            if (attribute == null || string.IsNullOrEmpty(attribute.Value))
            {
                return null;
            }

            string section = string.Empty;
            int questionNumber;
            string[] parts = attribute.Value.Split(';')?
                .FirstOrDefault()?
                .Split(new string[] { "::" }, StringSplitOptions.None);

            int reqParts = (subjectCode == "ELA") ? 5 : 4;
            if (parts == null || parts.Count() != reqParts)
            {
                return null;
            }

            if(subjectCode == "ELA")
            {
                section = parts.ElementAtOrDefault(3);
            }

            parts = parts.LastOrDefault()
                .Split(new string[] { "(", ")" }, StringSplitOptions.None);

            int.TryParse(parts.ElementAtOrDefault(1), out questionNumber);

            FieldTestUse fieldTestUse = new FieldTestUse
            {
                Code = parts.FirstOrDefault(),
                QuestionNumber = questionNumber,
                Section = section
            };

            return fieldTestUse;
        }
    }
}
