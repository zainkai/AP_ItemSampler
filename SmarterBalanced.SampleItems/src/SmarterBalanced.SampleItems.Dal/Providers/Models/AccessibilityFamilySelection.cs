using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilityFamilySelection
    {
        public string Code { get; }
        public string Label { get; }

        public AccessibilityFamilySelection(
            string code,
            string label)
        {
            Code = code;
            Label = label;
        }

        public static AccessibilityFamilySelection Create(
            string code = "",
            string label = "")
        {
            var sel = new AccessibilityFamilySelection(
                code: code,
                label: label);

            return sel;
        }

        public static AccessibilityFamilySelection Create(XElement xmlSelection)
        {
            var selection = new AccessibilityFamilySelection(
                    code: (string)xmlSelection.Element("Code"),
                    label: (string)xmlSelection.Element("Text")?.Element("Label"));

            return selection;
        }


    }
}
