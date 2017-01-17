using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AccessibilityResourceViewModel
    {
        public string SelectedCode { get; set; }

        public string DefaultCode { get; set; }

        public string Label { get; set; }

        public string Description { get; set; }

        public List<AccessibilitySelectionViewModel> Selections { get; set; }

        public bool Disabled { get; set; }

        public string ResourceTypeLabel { get; set; }
    }
}