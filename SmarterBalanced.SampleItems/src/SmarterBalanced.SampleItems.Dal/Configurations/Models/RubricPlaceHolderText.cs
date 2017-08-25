namespace SmarterBalanced.SampleItems.Dal.Configurations.Models
{
    /// <summary>
    /// Contains strings which should be filtered out of rubrics during translation.
    /// </summary>
    public class RubricPlaceHolderText
    {
        public string[] RubricPlaceHolderContains { get; set; }
        public string[] RubricPlaceHolderEquals { get; set; }
    }
}