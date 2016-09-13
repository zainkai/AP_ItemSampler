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

        [Display(Name = "ItemKey")]
        public int ItemKey { get; set; }

        public int? MinGrade { get; set; }

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
