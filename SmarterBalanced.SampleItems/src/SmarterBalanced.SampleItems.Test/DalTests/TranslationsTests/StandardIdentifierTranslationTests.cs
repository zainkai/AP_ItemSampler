﻿using SmarterBalanced.SampleItems.Dal.Translations;
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
        string invalidMathV1StandardString;
        string invalidMathV4StandardString;
        string invalidMathV5StandardString;
        string invalidMathV6StandardString;
        string noClaimString;

        public StandardIdentifierTranslationTests()
        {
            elaStandardString = "SBAC-ELA-v1:3-L|4-6|6.SL.2";
            mathv1StandardString = "SBAC-MA-v1:1|NBT|E-3|a/s|3.NBT.2";
            mathV4StandardString = "SBAC-MA-v4:1|NS|D-6|m|6.NS.6c";
            mathV5StandardString = "SBAC-MA-v5:2|NS|D-6|m|6.NS.6c";
            mathV6StandardString = "SBAC-MA-v6:3|P|TS04|D-6";
            invalidElaStandardString = "SBAC-ELA-v1:3-L";
            invalidMathV1StandardString = "SBAC-MA-v1:1|NBT||a/s|";
            invalidMathV4StandardString = "SBAC-MA-v4:|NS|D-6||";
            invalidMathV5StandardString = "SBAC-MA-v5:2||||6.NS.6c";
            invalidMathV6StandardString = "SBAC-MA-v6:3|||";
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
            Assert.Equal(string.Empty, identifier.Target);
            Assert.Equal(string.Empty, identifier.CommonCoreStandard);
        }

        [Fact]
        public void TestNoClaimString()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(noClaimString);
            Assert.Null(identifier);
        }

        [Fact]
        public void TestNullInput()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(null);
            Assert.Null(identifier);
        }

        [Fact]
        public void TestEmptyInput()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(string.Empty);
            Assert.Null(identifier);
        }

        [Fact]
        public void TestInvalidMathV1String()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(invalidMathV1StandardString);
            Assert.Equal("1", identifier.Claim);
            Assert.Equal(string.Empty, identifier.Target);
            Assert.Equal(string.Empty, identifier.CommonCoreStandard);
        }

        [Fact]
        public void TestInvalidMathV4String()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(invalidMathV4StandardString);
            Assert.Equal(string.Empty, identifier.Claim);
            Assert.Equal("D-6", identifier.Target);
            Assert.Equal("NS", identifier.ContentDomain);
            Assert.Equal(string.Empty, identifier.CommonCoreStandard);
            Assert.Equal(string.Empty, identifier.Emphasis);
        }

        [Fact]
        public void TestInvalidMathV5String()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(invalidMathV5StandardString);
            Assert.Equal("2", identifier.Claim);
            Assert.Equal("6.NS.6c", identifier.CommonCoreStandard);
            Assert.Equal(string.Empty, identifier.Target);
            Assert.Equal(string.Empty, identifier.ContentDomain);
            Assert.Equal(string.Empty, identifier.Emphasis);
        }

        [Fact]
        public void TestInvalidMathV6String()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.StandardStringtoStandardIdentifier(invalidMathV6StandardString);
            Assert.Equal("3", identifier.Claim);
            Assert.Equal(string.Empty, identifier.ContentCategory);
            Assert.Equal(string.Empty, identifier.TargetSet);
            Assert.Equal(string.Empty, identifier.TargetSet);
        }

        [Fact]
        public void TestElaCoreStandardToTarget()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.ElaCoreStandardToTarget(new string[] { "4-6", "3-6" });
            Assert.Equal("4-6", identifier.Claim);
            Assert.Equal("3-6", identifier.Target);
            Assert.Equal("ELA", identifier.SubjectCode);
        }

        [Fact]
        public void TestInvalidElaCoreStandardToTarget()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.ElaCoreStandardToTarget(new string[] { });
            Assert.Null(identifier);
        }

        [Fact]
        public void TestElaCoreStandardToCcss()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.ElaCoreStandardToCcss(new string[] { "4-6", "3-6", "6.SL.2" });
            Assert.Equal("4-6", identifier.Claim);
            Assert.Equal("3-6", identifier.Target);
            Assert.Equal("6.SL.2", identifier.CommonCoreStandard);
            Assert.Equal("ELA", identifier.SubjectCode);
        }

        [Fact]
        public void TestInvalidElaCoreStandardToCcss()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.ElaCoreStandardToCcss(new string[] { });
            Assert.Null(identifier);
        }

        [Fact]
        public void TestMathCoreStandardToTarget()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.MathCoreStandardToTarget(new string[] { "1", "NBT", "E-3" });
            Assert.Equal("1", identifier.Claim);
            Assert.Equal("NBT", identifier.ContentDomain);
            Assert.Equal("E-3", identifier.Target);
            Assert.Equal("MATH", identifier.SubjectCode);
        }

        [Fact]
        public void TestInvalidMathCoreStandardToTarget()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.MathCoreStandardToTarget(new string[] { });
            Assert.Null(identifier);
        }

        [Fact]
        public void TestMathCoreStandardToCcss()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.MathCoreStandardToCcss(new string[] { "4-6", "NS", "3-6", "m", "6.NS.6c" });
            Assert.Equal("4-6", identifier.Claim);
            Assert.Equal("3-6", identifier.Target);
            Assert.Equal("6.NS.6c", identifier.CommonCoreStandard);
            Assert.Equal("NS", identifier.ContentDomain);
            Assert.Equal("MATH", identifier.SubjectCode);
        }

        [Fact]
        public void TestInvalidMathCoreStandardToCcss()
        {
            StandardIdentifier identifier = StandardIdentifierTranslation.MathCoreStandardToCcss(new string[] { });
            Assert.Null(identifier);
        }

        [Fact]
        public void TestInvalidCoreStandardToIdentifier()
        {
            CoreStandardsRow coreStandardsRow = new CoreStandardsRow("", null, null, null, null, null);
            Exception exception = Assert.Throws<ArgumentException>(() => StandardIdentifierTranslation.CoreStandardToIdentifier(coreStandardsRow));
            Assert.Equal("The standards string must not be null or empty", exception.Message);
            exception = Assert.Throws<ArgumentException>(() => StandardIdentifierTranslation.CoreStandardToIdentifier(null));
            Assert.Equal("The standards string must not be null or empty", exception.Message);
        }

        [Fact]
        public void TestNullCoreStandardToIdentifier()
        {
            CoreStandardsRow coreStandardsRow = new CoreStandardsRow("", "3-l", null, null, null, null);
            StandardIdentifier identifier = StandardIdentifierTranslation.CoreStandardToIdentifier(coreStandardsRow);
            Assert.Null(identifier);
        }

        [Fact]
        public void TestCoreStandardToIdentifierELACcss()
        {
            CoreStandardsRow coreStandardsRow = new CoreStandardsRow("ELA", "3-l|4-6|6.SL.2", "", "", "CCSS", null);
            StandardIdentifier identifier = StandardIdentifierTranslation.CoreStandardToIdentifier(coreStandardsRow);
            Assert.Equal("3-l", identifier.Claim);
            Assert.Equal("4-6", identifier.Target);
            Assert.Equal("6.SL.2", identifier.CommonCoreStandard);
            Assert.Equal("ELA", identifier.SubjectCode);
        }

        [Fact]
        public void TestCoreStandardToIdentifierELATarget()
        {
            CoreStandardsRow coreStandardsRow = new CoreStandardsRow("ELA", "4-6|3-6", "", "", "Target", null);
            StandardIdentifier identifier = StandardIdentifierTranslation.CoreStandardToIdentifier(coreStandardsRow);
            Assert.Equal("4-6", identifier.Claim);
            Assert.Equal("3-6", identifier.Target);
            Assert.Equal("ELA", identifier.SubjectCode);
        }

        [Fact]
        public void TestCoreStandardToIdentifierMathCcss()
        {
            CoreStandardsRow coreStandardsRow = new CoreStandardsRow("MATH", "1|NBT|E-3|a/s|3.NBT.2", "", "", "CCSS", null);
            StandardIdentifier identifier = StandardIdentifierTranslation.CoreStandardToIdentifier(coreStandardsRow);
            Assert.Equal("1", identifier.Claim);
            Assert.Equal("NBT", identifier.ContentDomain);
            Assert.Equal("E-3", identifier.Target);
            Assert.Equal("3.NBT.2", identifier.CommonCoreStandard);
        }

        [Fact]
        public void TestCoreStandardToIdentifierMathTarget()
        {
            CoreStandardsRow coreStandardsRow = new CoreStandardsRow("MATH", "1|NBT|E-3", "", "", "Target", null);
            StandardIdentifier identifier = StandardIdentifierTranslation.CoreStandardToIdentifier(coreStandardsRow);
            Assert.Equal("1", identifier.Claim);
            Assert.Equal("NBT", identifier.ContentDomain);
            Assert.Equal("E-3", identifier.Target);
            Assert.Equal("MATH", identifier.SubjectCode);
        }
    }
}