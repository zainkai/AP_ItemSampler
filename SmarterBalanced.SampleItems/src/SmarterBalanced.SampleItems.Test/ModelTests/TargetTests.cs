using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.ModelTests
{
    class TargetTests
    {
        [Fact]
        public void TestTargetCreate()
        {
            var target = Target.Create(
                desc: "SHORT NAME: Long description",
                id: "1-2",
                idLabel: "1",
                subject: "ELA",
                claim: "1");
            Assert.Equal(target.Descripton, "Long description");
            Assert.Equal(target.Name, "Short Name");
            Assert.Equal(target.Id, "1-2");
            Assert.Equal(target.IdLabel, "1");
            Assert.Equal(target.ClaimId, "1");
            Assert.Equal(target.Subject, "ELA");
            Assert.Equal(target.NameHash, target.GetHashCode());
        }
    }
}
