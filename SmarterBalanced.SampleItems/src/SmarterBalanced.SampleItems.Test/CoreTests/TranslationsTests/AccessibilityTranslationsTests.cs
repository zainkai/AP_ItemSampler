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
    public class AccessibilityTranslationsTests
    {
        AccessibilityResourceGroup group1 = new AccessibilityResourceGroup("", 1,
                ImmutableArray.Create(
                    AccessibilityResource.Create(code: "AmericanSignLanguage", selectedCode: "TDS_ASL0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ASL0", "", 1, false),
                        new AccessibilitySelection("TDS_ASL1", "", 2, false))),
                    AccessibilityResource.Create(code: "ColorContrast", selectedCode: "TDS_CC0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_CC0", "", 1, false),
                        new AccessibilitySelection("TDS_CCInvert", "", 2, false)))));

        AccessibilityResourceGroup group2 = new AccessibilityResourceGroup("", 2,
                ImmutableArray.Create(
                    AccessibilityResource.Create(code: "ClosedCaptioning", selectedCode: "TDS_ClosedCap0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ClosedCap0", "", 1, false),
                        new AccessibilitySelection("TDS_ClosedCap1", "", 2, false))),

                    AccessibilityResource.Create(code: "Language", selectedCode: "ENU", selections: ImmutableArray.Create(
                        new AccessibilitySelection("ENU", "", 1, false),
                        new AccessibilitySelection("ESN", "", 2, false)))));

        ImmutableArray<AccessibilityResourceGroup> groups;

        Dictionary<string, string> cookie = new Dictionary<string, string>()
        {
            //{"AmericanSignLanguage", "TDS_ASL0" },
            {"ColorContrast", "TDS_CCInvert" },
            {"ClosedCaptioning", "TDS_ClosedCap1" },
            //{"Language", "ENU" }
        };

        string[] isaap = new string[] {
            "TDS_ASL1",
            "ESN",
        };

        AccessibilityResource familyResource = new AccessibilityResource(
            code: "TDS_CC",
            selectedCode: "TDS_CC0",
            order: 0,
            defaultSelection: "TDS_CC0",
            selections: ImmutableArray.Create(new AccessibilitySelection[] { }),
            label: "familyResource",
            description: "familyResource",
            disabled: false,
            resourceType: "familyResource Type");

        AccessibilityResource globalResource = new AccessibilityResource(
            code: "TDS_CC",
            selectedCode: "TDS_CC0",
            order: 5,
            defaultSelection: "TDS_CC0",
            selections: ImmutableArray.Create(
                new AccessibilitySelection("TDS_CC0", "Black on White", 2, false),
                new AccessibilitySelection("TDS_CCInvert", "Reverse Contrast", 2, false),
                new AccessibilitySelection("TDS_CCMagenta", "Black on Rose", 2, false),
                new AccessibilitySelection("TDS_CCMedGrayLtGray", "Medium Gray on Light Gray", 2, false)),
            label: "globalResource",
            description: "globalResource",
            disabled: false,
            resourceType: "globalResource Type");

        AccessibilitySelection nullLabelSelection = new AccessibilitySelection("TDS_CC0", null, 1, false);
        AccessibilitySelection goodSelection = new AccessibilitySelection("TDS_CCInvert", "Good Selection", 1, false);
        AccessibilitySelection disabledSelection = new AccessibilitySelection("TDS_CCMedGrayLtGray", "Disabled Selection", 1, true);
        AccessibilitySelection goodSelectionOtherCode = new AccessibilitySelection("TestCode", "Good Selection Other Code", 1, false);

        ImmutableArray<AccessibilitySelection> emptySelection = ImmutableArray<AccessibilitySelection>.Empty;

        AccessibilityResource happyFamilyResource = new AccessibilityResource(
            code: "familyResource",
            selectedCode: "TDS_CCInvert",
            order: 0,
            defaultSelection: "TDS_CC0",
            selections: ImmutableArray.Create(new AccessibilitySelection[] { }),
            label: "familyResource",
            description: "familyResource",
            disabled: false,
            resourceType: "familyResource Type");

        public AccessibilityTranslationsTests()
        {
            groups = ImmutableArray.Create(group1, group2);
        }

        #region ApplyPreferences

        private string SelectedCode(ImmutableArray<AccessibilityResourceGroup> result, string resourceCode)
        {
            var resources = new List<AccessibilityResource>();
            foreach(var rg in result)
            {
                resources.AddRange(rg.AccessibilityResources);
            }
            return resources.FirstOrDefault(r => r.Code == resourceCode).SelectedCode;
        }

        [Fact]
        public void TestApplyPreferencesNoIsaapNoCookie()
        {
            var result=groups.ApplyPreferences(new string[] { }, new Dictionary<string, string>());

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

        #region MergeWith

        [Fact]
        public void TestMergeWithHappyCase()
        {
            var newFamilyRes = familyResource.WithSelections(familyResource.Selections
                    .Append(goodSelection)
                    .ToImmutableArray());
            var merged = newFamilyRes.MergeWith(globalResource);
            
            Assert.Equal(merged.Code, "TDS_CC");
            Assert.Equal(merged.Description, globalResource.Description);
            Assert.Equal(merged.Label, newFamilyRes.Label);
            Assert.Equal(merged.DefaultSelection, "TDS_CCInvert");
            Assert.Equal(merged.Order, globalResource.Order);
            Assert.Equal(merged.Disabled, newFamilyRes.Disabled);
            var res = merged.Selections.Where(r => r.Label == goodSelection.Label).ToList();
            Assert.Equal(1, res.Count);
            Assert.Equal(4, merged.Selections.Count());
        }

        [Fact]
        public void TestMergeWithExtraSelection()
        {
            var newFamilyRes = familyResource.WithSelections(familyResource.Selections
                    .Append(goodSelectionOtherCode)
                    .ToImmutableArray());

            var merged = newFamilyRes.MergeWith(globalResource);
            Assert.DoesNotContain(merged.Selections, s => s.Label == goodSelectionOtherCode.Label);
            Assert.DoesNotContain(merged.Selections, s => !s.Disabled);
        }

        [Fact]
        public void TestMergeWithUseDefaults()
        {
            var newFamilyRes = AccessibilityResource.Create(code: "hello", selections: emptySelection
                    .Append(goodSelection).ToImmutableArray());
            var merged = newFamilyRes.MergeWith(globalResource);

            Assert.Equal(merged.Code, globalResource.Code);
            Assert.Equal(merged.Description, globalResource.Description);
            Assert.Equal(merged.Label, globalResource.Label);
            Assert.Equal(merged.DefaultSelection, goodSelection.Code);
            Assert.Equal(merged.Order, globalResource.Order);
            Assert.Equal(merged.Disabled, globalResource.Disabled);
            Assert.Equal(4, merged.Selections.Count());
        }

        [Fact]
        public void TestMergeWithDisabledSelection()
        {

            var newFamilyRes = happyFamilyResource.WithSelections(familyResource.Selections
                    .Append(disabledSelection)
                    .Append(goodSelection)
                    .ToImmutableArray());

            var merged = newFamilyRes.MergeWith(globalResource);
            Assert.Equal(4, merged.Selections.Length);

            var disabledSelections = merged.Selections.Where(r => r.Disabled).ToList();
            Assert.Equal(3, disabledSelections.Count);
            Assert.Contains(disabledSelections, s => s.Code == disabledSelection.Code);
        }

        [Fact]
        public void TestMergeWithThrowsOnNulls()
        {
            Assert.Throws<ArgumentNullException>(() => AccessibilityResourceTranslation.MergeWith(familyResource, null));
            Assert.Throws<ArgumentNullException>(() => AccessibilityResourceTranslation.MergeWith(null, globalResource));
            Assert.Throws<ArgumentNullException>(() => AccessibilityResourceTranslation.MergeWith(null, null));
        }

        #endregion

        #region MergeSelection

        [Fact]
        public void TestMergeSelectionHappyCase()
        {
            var newFamilyRes = familyResource.WithSelections(familyResource.Selections
                    .Append(goodSelection)
                    .ToImmutableArray());
            var globalSelection = globalResource.Selections.FirstOrDefault(s => s.Code == goodSelection.Code);
            var merged = AccessibilityResourceTranslation.MergeSelection(globalSelection, newFamilyRes);
            var disabled = goodSelection.Disabled || newFamilyRes.Disabled || newFamilyRes.Selections.FirstOrDefault().Disabled;
            var familySel = newFamilyRes.Selections[0];
            Assert.Equal(goodSelection.Code, merged.Code);
            Assert.Equal(disabled, merged.Disabled);
            Assert.Equal(familySel.Label, merged.Label);
            Assert.Equal(familySel.Order, merged.Order);
        }

        [Fact(Skip = "TODO: talk to Alex. There are tests with the same name.")]
        public void TestMergeSelectionVoidProps()
        {
            var familySel = AccessibilitySelection.Create(code:goodSelection.Code);
            var newFamilyRes = familyResource.WithSelections(emptySelection
                    .Append(familySel)
                    .ToImmutableArray());
            var globalSelection = globalResource.Selections.FirstOrDefault(s => s.Code == goodSelection.Code);
            var merged = AccessibilityResourceTranslation.MergeSelection(globalSelection, newFamilyRes);
           
            Assert.Equal(goodSelection.Code, merged.Code);
            Assert.False(merged.Disabled);
            Assert.Equal(globalSelection.Label, merged.Label);
            Assert.Equal(familySel.Order, merged.Order);
        }

        #endregion
    }

}

