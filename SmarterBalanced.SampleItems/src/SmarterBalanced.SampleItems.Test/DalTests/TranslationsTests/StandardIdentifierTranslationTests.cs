using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.DalTests.TranslationsTests
{
    /// <summary>
    /// Tests the translation of a standards string into a StandardIdentifier object
    /// Different publications use different standard ids
    /// This tests the translation of standards listed in the SmarterApp "Interpreting Smarter Balances Standard Ids" document.
    /// http://www.smarterapp.org/documents/InterpretingSmarterBalancedStandardIDs.html
    /// </summary>
    public class StandardIdentifierTranslationTests
    {
        string elaStandardString;
        string mathv1StandardString;
        string mathV4StandardString;
        string mathV5StandardString;
        string mathV6StandardString;
        string invalidElaStandardString;
        string noClaimString;

        public StandardIdentifierTranslationTests()
        {
            elaStandardString = "SBAC-ELA-v1:3-L|4-6|6.SL.2";
            mathv1StandardString = "SBAC-MA-v1:1|NBT|E-3|a/s|3.NBT.2";
            mathV4StandardString = "SBAC-MA-v4:1|NS|D-6|m|6.NS.6c";
            mathV5StandardString = "SBAC-MA-v5:2|NS|D-6|m|6.NS.6c";
            mathV6StandardString = "SBAC-MA-v6:3|P|TS04|D-6";
            invalidElaStandardString = "SBAC-ELA-v1:3-L|4-6";
            noClaimString = "SBAC-ELA-v1|4-6|6.SL.2";
        }

        [Fact]
        public void TestElaV1Translation()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(elaStandardString);
            Assert.Equal("3-L", identifier.Claim);
            Assert.Equal("4-6", identifier.Target);
            Assert.Equal("6.SL.2", identifier.CommonCoreStandard);
        }

        [Fact]
        public void TestMathV1Translation()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(mathv1StandardString);
            Assert.Equal("1", identifier.Claim);
            Assert.Equal("NBT", identifier.ContentDomain);
            Assert.Equal("E-3", identifier.Target);
            Assert.Equal("3.NBT.2", identifier.CommonCoreStandard);
        }

        [Fact]
        public void TestMathV4Translation()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(mathV4StandardString);
            Assert.Equal("1", identifier.Claim);
            Assert.Equal("D-6", identifier.Target);
            Assert.Equal("6.NS.6c", identifier.CommonCoreStandard);
            Assert.Equal("NS", identifier.ContentDomain);
            Assert.Equal("m", identifier.Emphasis);
        }

        [Fact]
        public void TestMathV5Translation()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(mathV5StandardString);
            Assert.Equal("2", identifier.Claim);
            Assert.Equal("D-6", identifier.Target);
            Assert.Equal("6.NS.6c", identifier.CommonCoreStandard);
            Assert.Equal("NS", identifier.ContentDomain);
            Assert.Equal("m", identifier.Emphasis);
        }

        [Fact]
        public void TestMathV6Translation()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(mathV6StandardString);
            Assert.Equal("3", identifier.Claim);
            Assert.Equal("D-6", identifier.Target);
            Assert.Equal("P", identifier.ContentCategory);
            Assert.Equal("TS04", identifier.TargetSet);
        }

        [Fact]
        public void TestInvalidElaString()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(invalidElaStandardString);
            Assert.Equal("3-L", identifier.Claim);
            Assert.Null(identifier.Target);
            Assert.Null(identifier.CommonCoreStandard);
        }

        [Fact]
        public void TestNoClaimString()
        {
            Assert.Throws<ArgumentException>(() => StandardIdentifierTranslation.StandardStringtoStandardIdentifier(noClaimString));
        }

        [Fact]
        public void TestNullInput()
        {
            Assert.Throws<ArgumentException>( () => StandardIdentifierTranslation.StandardStringtoStandardIdentifier(null));
        }

        [Fact]
        public void TestEmptyInput()
        {
            Assert.Throws<ArgumentException>(() => StandardIdentifierTranslation.StandardStringtoStandardIdentifier(string.Empty));
        }

        //TODO: Add invalid strings for each of the ma and ela types
        //TODO: add standards testing
    }
}
