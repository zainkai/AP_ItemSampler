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

namespace SmarterBalanced.SampleItems.Test.CoreTests.TranslationsTests
{
    public class AccessibilityTranslationsTests
    {
        ImmutableArray<AccessibilityResourceGroup> groups;
        AccessibilityResourceGroup group1, group2;
        Dictionary<string, string> cookie;
        string[] isaap;
        public AccessibilityTranslationsTests()
        {
            cookie = new Dictionary<string, string>()
            {
                {"AmericanSignLanguage", "TDS_ASL0" },
                {"ColorContrast", "TDS_CCInvert" },
                {"ClosedCaptioning", "TDS_ClosedCap1" },
                {"Language", "ENU" }
            };

            //accessibilitySelections = new List<AccessibilitySelection>
            //{
            //    new AccessibilitySelection()
            //    {
            //        Code = "TDS_TEST1",
            //        Order = 1,
            //        Label = "TEST 1",
            //        Disabled = false
            //    },
            //    new AccessibilitySelection()
            //    {
            //        Code = "TDS_TEST2",
            //        Order = 2,
            //        Label = "TEST 2",
            //        Disabled = false
            //    },
            //    new AccessibilitySelection()
            //    {
            //        Code = "TDS_TEST3",
            //        Order = 3,
            //        Label = "TEST 3",
            //        Disabled = false
            //    }
            //};
            group1 = new AccessibilityResourceGroup("", 1,
                ImmutableArray.Create(
                    AccessibilityResource.Create(code: "AmericanSignLanguage", selectedCode: "TDS_ASL0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ASL0", "", 1, false), 
                        new AccessibilitySelection("TDS_ASL1", "", 2, false))),
                    AccessibilityResource.Create(code: "ColorContrast", selectedCode: "TDS_CC0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_CC0", "", 1, false),
                        new AccessibilitySelection("TDS_CCInvert", "", 2, false)))));
            group2 = new AccessibilityResourceGroup("", 2,
                ImmutableArray.Create(
                    AccessibilityResource.Create(code: "ClosedCaptioning", selectedCode: "TDS_ClosedCap0", selections: ImmutableArray.Create(
                        new AccessibilitySelection("TDS_ClosedCap0", "", 1, false),
                        new AccessibilitySelection("TDS_ClosedCap1", "", 2, false))),

                    AccessibilityResource.Create(code: "Language", selectedCode: "ENU", selections: ImmutableArray.Create(
                        new AccessibilitySelection("ENU", "", 1, false),
                        new AccessibilitySelection("ESN", "", 2, false)))));
            groups = ImmutableArray.Create(group1, group2);


            //accessibilityResources = new List<AccessibilityResource>
            //{
            //    new AccessibilityResource()
            //    {
            //        Order = 1,
            //        DefaultSelection = null,
            //        Selections = accessibilitySelections,
            //        Label = "Resource 1",
            //        Disabled = false
            //    },
            //    new AccessibilityResource()
            //    {
            //        Order = 2,
            //        DefaultSelection = "TDS_TEST1",
            //        Selections = accessibilitySelections,
            //        Label = "Resource 2",
            //        Disabled = false
            //    }
            //};

            //accessibilityResourceViewModels = new List<AccessibilityResourceViewModel>
            //{
            //    new AccessibilityResourceViewModel()
            //    {
            //        DefaultCode = "TDS_TEST1",
            //        SelectedCode = "TDS_TEST1",
            //        Selections = accessibilitySelections,
            //        Label = "Resource 1",
            //        Disabled = false
            //    },
            //    new AccessibilityResourceViewModel()
            //    {
            //        DefaultCode = "TDS_TEST2",
            //        SelectedCode = "TDS_TEST2",
            //        Selections = accessibilitySelections,
            //        Label = "Resource 2",
            //        Disabled = false
            //    }
            //};
        }

        [Fact]
        public void TestApplyPreferencesNoIsaapNoCookie()
        {
            var result=groups.ApplyPreferences(new string[] { }, new Dictionary<string, string>());

            //TODO: figure out what should be happening to the data
        }

        [Fact]
        public void TestApplyPreferencesNoIsaapYesCookie()
        {
            var result = groups.ApplyPreferences(new string[] { }, cookie);
            
            //check to make sure that the second and third accessibility prefs are changed, but not 1 and 4
        }
    }

}

