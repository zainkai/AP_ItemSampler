using Microsoft.AspNetCore.Mvc.Rendering;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Immutable;
using SmarterBalanced.SampleItems.Dal.Translations;

namespace SmarterBalanced.SampleItems.Test.CoreTests.TranslationsTests
{
    public class AccessibilityGroupTranslationsTests
    {
        AccessibilityResourceGroup group1 = new AccessibilityResourceGroup("", 1,
                ImmutableArray.Create(
                    AccessibilityResource.Create(resourceCode: "AmericanSignLanguage", currentSelectionCode: "TDS_ASL0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ASL0", "", 1, false, false),
                        new AccessibilitySelection("TDS_ASL1", "", 2, false, false))),
                    AccessibilityResource.Create(resourceCode: "ColorContrast", currentSelectionCode: "TDS_CC0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_CC0", "", 1, false, false),
                        new AccessibilitySelection("TDS_CCInvert", "", 2, false, false)))));

        AccessibilityResourceGroup group2 = new AccessibilityResourceGroup("", 2,
                ImmutableArray.Create(
                    AccessibilityResource.Create(resourceCode: "ClosedCaptioning", currentSelectionCode: "TDS_ClosedCap0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ClosedCap0", "", 1, false, false),
                        new AccessibilitySelection("TDS_ClosedCap1", "", 2, false, false))),

                    AccessibilityResource.Create(resourceCode: "Language", currentSelectionCode: "ENU", selections: ImmutableArray.Create(
                        new AccessibilitySelection("ENU", "", 1, false, false),
                        new AccessibilitySelection("ESN", "", 2, false, false)))));

        ImmutableArray<AccessibilityResourceGroup> groups;

        Dictionary<string, string> cookie = new Dictionary<string, string>()
        {
            {"ColorContrast", "TDS_CCInvert" },
            {"ClosedCaptioning", "TDS_ClosedCap1" },
        };

        string[] isaap = new string[] {
            "TDS_ASL1",
            "ESN",
        };

        public AccessibilityGroupTranslationsTests()
        {
            groups = ImmutableArray.Create(group1, group2);
        }

        #region ApplyPreferences

        private string SelectedCode(ImmutableArray<AccessibilityResourceGroup> result, string resourceCode)
        {
            var resources = new List<AccessibilityResource>();
            foreach (var rg in result)
            {
                resources.AddRange(rg.AccessibilityResources);
            }
            return resources.FirstOrDefault(r => r.ResourceCode == resourceCode).CurrentSelectionCode;
        }

        [Fact]
        public void TestApplyPreferencesNoIsaapNoCookie()
        {
            var result = groups.ApplyPreferences(new string[] { }, new Dictionary<string, string>());

            Assert.NotNull(result);
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL0");
            Assert.Equal(SelectedCode(result, "ColorContrast"), "TDS_CC0");
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap0");
            Assert.Equal(SelectedCode(result, "Language"), "ENU");
        }

        [Fact]
        public void TestApplyPreferencesNoIsaapYesCookie()
        {
            var result = groups.ApplyPreferences(new string[] { }, cookie);

            Assert.NotNull(result);

            //check to make sure that the second and third accessibility prefs are changed, but not 1 and 4
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL0");
            Assert.Equal(SelectedCode(result, "ColorContrast"), "TDS_CCInvert");//changed
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap1");//changed
            Assert.Equal(SelectedCode(result, "Language"), "ENU");
        }

        [Fact]
        public void TestApplyPreferencesYesIsaapNoCookie()
        {
            var result = groups.ApplyPreferences(isaap, new Dictionary<string, string>());

            Assert.NotNull(result);

            //check to make sure that the 1st and 4th accessibility prefs are changed, but not 2 and 3
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL1");//changed
            Assert.Equal(SelectedCode(result, "ColorContrast"), "TDS_CC0");
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap0");
            Assert.Equal(SelectedCode(result, "Language"), "ESN");//changed
        }

        [Fact]
        public void TestApplyPreferencesYesIsaapYesCookie()
        {
            var result = groups.ApplyPreferences(isaap, cookie);

            Assert.NotNull(result);

            //check to make sure that the 1st and 4th accessibility prefs are changed, but not 2 and 3
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL1");//changed
            Assert.Equal(SelectedCode(result, "ColorContrast"), "TDS_CC0");
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap0");
            Assert.Equal(SelectedCode(result, "Language"), "ESN");//changed
        }

        #endregion

        //TODO: add test for having cookie and/or issap cookie having an option
        //then applying it to a group that has the option disabled.

    }


}

