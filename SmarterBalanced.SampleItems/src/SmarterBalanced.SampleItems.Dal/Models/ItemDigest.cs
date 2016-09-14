using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Dal.Models
{
    public class ItemDigest : IEquatable<ItemDigest>
    {
        [Display(Name="Bank")]
        public int BankKey { get; set; }

        [Display(Name = "Item Key")]
        public int ItemKey { get; set; }

        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Display(Name = "Grade")]
        public int Grade { get; set; }

        [Display(Name = "Claim")]
        public string Claim { get; set; }

        [Display(Name = "Target Assessment Type")]
        public string Target { get; set; }

        [Display(Name = "Interaction Type")]
        public string InteractionType { get; set; }

        public int? MaxGrade { get; set; }

        public int? TargetedGrade { get; set; }

        public string SubjectCode { get; set; }

        #region Helper Methods

        public override string ToString()
        {
            return $"{BankKey}-{ItemKey}";
        }
        public bool Equals(ItemDigest obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return ($"{BankKey}-{ItemKey}").GetHashCode();
        }

        #endregion

    }

}
