using Microsoft.AspNetCore.Mvc.Rendering;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Core.Translations;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.CoreTests.TranslationsTests
{
    public class AccessibilityTranslationsTests
    {
        List<AccessibilityResource> accessibilityResources;
        List<AccessibilitySelection> accessibilitySelections;
        List<SelectListItem> accSelectListItems;
        List<AccessibilityResourceViewModel> accessibilityResourceViewModels;

        public AccessibilityTranslationsTests()
        {
            accessibilitySelections = new List<AccessibilitySelection>
            {
                new AccessibilitySelection()
                {
                    Code = "TDS_TEST1",
                    Order = 1,
                    Label = "TEST 1",
                    Disabled = false
                },
                new AccessibilitySelection()
                {
                    Code = "TDS_TEST2",
                    Order = 2,
                    Label = "TEST 2",
                    Disabled = false
                },
                new AccessibilitySelection()
                {
                    Code = "TDS_TEST3",
                    Order = 3,
                    Label = "TEST 3",
                    Disabled = false
                }
            };

            accSelectListItems = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Value = "TDS_TEST1",
                    Text = "TEST 1",
                    Disabled = false
                },
                new SelectListItem()
                {
                    Value = "TDS_TEST2",
                    Text = "TEST 2",
                    Disabled = false
                },
                new SelectListItem()
                {
                    Value = "TDS_TEST3",
                    Text = "TEST 3",
                    Disabled = false
                }
            };

            accessibilityResources = new List<AccessibilityResource>
            {
                new AccessibilityResource()
                {
                    Order = 1,
                    DefaultSelection = null,
                    Selections = accessibilitySelections,
                    Label = "Resource 1",
                    Disabled = false
                },
                new AccessibilityResource()
                {
                    Order = 2,
                    DefaultSelection = "TDS_TEST1",
                    Selections = accessibilitySelections,
                    Label = "Resource 2",
                    Disabled = false
                }
            };
        }

        [Fact(Skip = "Not implemented")]
        public void TestItemsToISAAP()
        {
            throw new NotImplementedException();
        }

        [Fact(Skip = "Not implemented")]
        public void TestISAAPtoItems()
        {
            throw new NotImplementedException();
        }


        [Fact]
        public void TestAccessibilityResourceToViewModel()
        {
            AccessibilityResource accResource = accessibilityResources[0];
            List<AccessibilityResource> accResources = new List<AccessibilityResource>
            {
                accResource
            };

            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accResources);

            Assert.Equal(accResources.Count(), accResourceViewModels.Count());

            AccessibilityResourceViewModel accResourceViewModel = accResourceViewModels[0];

            Assert.Equal(accResource.Code, accResourceViewModel.SelectedCode);
            Assert.Equal(accResource.Label, accResourceViewModel.Label);
            Assert.Equal(accResource.Disabled, accResourceViewModel.Disabled);
        }

        [Fact]
        public void TestAccessibilityResourceToViewModelWithISAAP()
        {
            AccessibilityResource accResource = accessibilityResources[0];
            List<AccessibilityResource> accResources = new List<AccessibilityResource>
            {
                accResource
            };
            string ISAAP = "TDS_TEST3;";
            string ExpectedCode = ISAAP.Split(';')[0];
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accResources, ISAAP);

            Assert.Equal(accResources.Count(), accResourceViewModels.Count());

            AccessibilityResourceViewModel accResourceViewModel = accResourceViewModels[0];

            Assert.Equal(ExpectedCode, accResourceViewModel.SelectedCode);
            Assert.Equal(accResource.Label, accResourceViewModel.Label);
            Assert.Equal(accResource.Disabled, accResourceViewModel.Disabled);
        }

        [Fact]
        public void TestAccessibilityResourceToViewModelsWithISAAP()
        {
            string ISAAP = "TDS_TEST3;";
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accessibilityResources, ISAAP);

            Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }


        [Fact]
        public void TestAccessibilityResourcesToViewModels()
        {
            List<AccessibilityResourceViewModel> accResourceViewModels = 
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accessibilityResources);

            Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }

    }

}

