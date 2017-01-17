namespace SmarterBalanced.SampleItems.Core.Repos.Models
{
    public class AccessibilitySelectionViewModel
    {
        public string Code { get; }
        public string Label { get; }
        public bool Disabled { get; }

        public AccessibilitySelectionViewModel(string code, string label, bool disabled)
        {
            Code = code;
            Label = label;
            Disabled = disabled;
        }
    }
}