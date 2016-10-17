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
        public List<SelectListItem> AccessibilityListItems { get; set; }

        public bool Disabled { get; set; }
    }
}