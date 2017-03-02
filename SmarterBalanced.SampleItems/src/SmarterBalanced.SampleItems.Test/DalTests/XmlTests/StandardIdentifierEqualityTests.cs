using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.DalTests.XmlTests
{
    public class StandardIdentifierEqualityTests
    {
        private string claim1 = "c1";

        private string subjectMath = "MATH";
        private string subjectEla = "ELA";

        private string contentDomain1 = "cd1";
        private string contentDomain2 = "cd2";

        private string target1 = "t1";
        private string target2 = "t2";

        private string emphasis1 = "e1";
        private string emphasis2 = "e2";

        private string Ccss1 = "ccss1";
        private string Ccss2 = "ccss2";

        private StandardIdentifier DefaultWith(
            string subjectCode,
            string claim = null,
            string target = null,
            string contentDomain = null,
            string contentCategory = null,
            string emphasis = null,
            string commonCoreStandard = null)
        {
            return StandardIdentifier.Create(
                subjectCode: subjectCode,
                claim: claim ?? claim1,
                contentDomain: contentDomain ?? contentDomain1,
                contentCategory: contentCategory ?? contentDomain1,
                target: target ?? target1,
                emphasis: emphasis ?? emphasis1,
                commonCoreStandard: commonCoreStandard ?? Ccss1
                );
        }

        [Fact]
        public void TestMathEqual()
        {
            StandardIdentifier x = DefaultWith(subjectMath);
            StandardIdentifier y = DefaultWith(subjectMath);
            Assert.True(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.True(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestMathNotEqualTarget()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectMath);

            StandardIdentifier y = DefaultWith(subjectCode: subjectMath, target: target2);

            Assert.False(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.False(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestMathNotEqualContentDomain()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectMath);
            StandardIdentifier y = DefaultWith(subjectCode: subjectMath, contentDomain: contentDomain2);

            Assert.False(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.False(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestMathNotEqualEmphasis()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectMath);
            StandardIdentifier y = DefaultWith(subjectCode: subjectMath, contentDomain: emphasis2);

            Assert.False(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.False(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestMathNotEqualCCSS()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectMath);
            StandardIdentifier y = DefaultWith(subjectCode: subjectMath, commonCoreStandard: Ccss2);

            Assert.False(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.True(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestElaEqual()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectEla);
            StandardIdentifier y = DefaultWith(subjectCode: subjectEla);

            Assert.True(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.True(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestElaNotEqualTarget()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectEla);
            StandardIdentifier y = DefaultWith(subjectCode: subjectEla, target: target2);

            Assert.False(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.False(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

        [Fact]
        public void TestElaNotEqualCcss()
        {
            StandardIdentifier x = DefaultWith(subjectCode: subjectEla);
            StandardIdentifier y = DefaultWith(subjectCode: subjectEla, commonCoreStandard: Ccss2);

            Assert.False(StandardIdentifierCcssComparer.Instance.Equals(x, y));
            Assert.True(StandardIdentifierTargetComparer.Instance.Equals(x, y));
        }

    }


}
