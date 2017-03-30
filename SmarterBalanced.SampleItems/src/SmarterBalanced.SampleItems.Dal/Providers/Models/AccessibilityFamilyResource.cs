using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SmarterBalanced.SampleItems.Dal.Providers.Models
{
    public class AccessibilityFamilyResource
    {
        /// <summary>
        /// ID for this accessibility resource.
        /// </summary>
        public string ResourceCode { get; }
        public bool Disabled { get; }
        public string DefaultSelection { get; }

        public ImmutableArray<AccessibilityFamilySelection> Selections { get; }

        public AccessibilityFamilyResource(
            string resourceCode,
            ImmutableArray<AccessibilityFamilySelection> selections,
            bool disabled,
            string defaultSelection)
        {
            ResourceCode = resourceCode;
            Selections = selections;
            Disabled = disabled;
            DefaultSelection = defaultSelection;
        }

        public static AccessibilityFamilyResource Create(XElement elem)
        {
            var selectionsElem = elem.Elements("Selection");
            var selections = selectionsElem == null
                ? ImmutableArray<AccessibilityFamilySelection>.Empty
                : selectionsElem.Select(s => AccessibilityFamilySelection.Create(s)).ToImmutableArray();

            var resource = new AccessibilityFamilyResource(
                resourceCode: (string)elem.Element("Code"),
                disabled: elem.Element("Disabled") != null,
                selections: selections,
                defaultSelection: (string)elem.Element("DefaultSelection"));

            return resource;
        }
    }
}
