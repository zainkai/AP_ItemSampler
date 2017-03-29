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
                        new AccessibilitySelection("TDS_ASL0", "", 1, false),
                        new AccessibilitySelection("TDS_ASL1", "", 2, false))),
                    AccessibilityResource.Create(resourceCode: "ColorContrast", currentSelectionCode: "TDS_CC0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_CC0", "", 1, false),
                        new AccessibilitySelection("TDS_CCInvert", "", 2, false)))));

        AccessibilityResourceGroup group2 = new AccessibilityResourceGroup("", 2,
                ImmutableArray.Create(
                    AccessibilityResource.Create(resourceCode: "ClosedCaptioning", currentSelectionCode: "TDS_ClosedCap0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ClosedCap0", "", 1, false),
                        new AccessibilitySelection("TDS_ClosedCap1", "", 2, false))),

                    AccessibilityResource.Create(resourceCode: "Language", currentSelectionCode: "ENU", selections: ImmutableArray.Create(
                        new AccessibilitySelection("ENU", "", 1, false),
                        new AccessibilitySelection("ESN", "", 2, false)))));

        AccessibilityResourceGroup group3 = new AccessibilityResourceGroup("", 2,
        ImmutableArray.Create(
            AccessibilityResource.Create(resourceCode: "ClosedCaptioning", currentSelectionCode: "TDS_ClosedCap0", disabled: true, selections: ImmutableArray.Create(
                new AccessibilitySelection("TDS_ClosedCap0", "", 1, true),
                new AccessibilitySelection("TDS_ClosedCap1", "", 2, true))),
            AccessibilityResource.Create(resourceCode: "Language", currentSelectionCode: "ENU", disabled: true, selections: ImmutableArray.Create(
                new AccessibilitySelection("ENU", "", 1, true),
                new AccessibilitySelection("ESN", "", 2, true))),
            AccessibilityResource.Create(resourceCode: "AmericanSignLanguage", currentSelectionCode: "TDS_ASL0", selections: ImmutableArray.Create(
                new AccessibilitySelection("TDS_ASL0", "", 1, false),
                new AccessibilitySelection("TDS_ASL1", "", 2, false)))));

        ImmutableArray<AccessibilityResourceGroup> groups;
        ImmutableArray<AccessibilityResourceGroup> groupsDisabledOptions;

        Dictionary<string, string> cookie = new Dictionary<string, string>()
        {
            {"ColorContrast", "TDS_CCInvert" },
            {"ClosedCaptioning", "TDS_ClosedCap1" },
        };

        Dictionary<string, string> badCookie = new Dictionary<string, string>()
        {
            {"Language", "ENU" },
            {"ClosedCaptioning", "TDS_ClosedCap0" },
        };

        string[] isaap = new string[] {
            "TDS_ASL1",
            "ESN",
        };

        string[] badIsaap = new string[] {
            "TDS_ASL0",
            "ESN",
        };

        public AccessibilityGroupTranslationsTests()
        {
            groups = ImmutableArray.Create(group1, group2);
            groupsDisabledOptions = ImmutableArray.Create(group3);

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

        [Fact]
        public void TestApplyPreferencesCookieOptionDisabled()
        {
            var result = groupsDisabledOptions.ApplyPreferences(new string[] { }, badCookie);

            Assert.NotNull(result);

            //applying cookie to a group that has the option disabled.
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL0");
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap0");
            Assert.Equal(SelectedCode(result, "Language"), "ENU");
            Assert.Equal(result[0].AccessibilityResources[0].Disabled, true);//Assert option is still disabled
            Assert.Equal(result[0].AccessibilityResources[1].Disabled, true);
            Assert.Equal(result[0].AccessibilityResources[2].Disabled, false);
        }

        [Fact]
        public void TestApplyPreferencesIsaapOptionDisabled()
        {
            var result = groupsDisabledOptions.ApplyPreferences(isaap, new Dictionary<string, string>());

            Assert.NotNull(result);

            //applying cookie to a group that has the option disabled.
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL1");
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap0");
            Assert.Equal(SelectedCode(result, "Language"), "ENU");
            Assert.Equal(result[0].AccessibilityResources[0].Disabled, true);//Assert option is still disabled
            Assert.Equal(result[0].AccessibilityResources[1].Disabled, true);
            Assert.Equal(result[0].AccessibilityResources[2].Disabled, false);
        }

        [Fact]
        public void TestApplyPreferencesIsaapCookieOptionDisabled()
        {
            var result = groupsDisabledOptions.ApplyPreferences(badIsaap, badCookie);

            Assert.NotNull(result);

            //applying cookie to a group that has the option disabled.
            Assert.Equal(SelectedCode(result, "AmericanSignLanguage"), "TDS_ASL0");
            Assert.Equal(SelectedCode(result, "ClosedCaptioning"), "TDS_ClosedCap0");
            Assert.Equal(SelectedCode(result, "Language"), "ENU");
            Assert.Equal(result[0].AccessibilityResources[0].Disabled, true);//Assert option is still disabled
            Assert.Equal(result[0].AccessibilityResources[1].Disabled, true);
            Assert.Equal(result[0].AccessibilityResources[2].Disabled, false);
        }
    }
}