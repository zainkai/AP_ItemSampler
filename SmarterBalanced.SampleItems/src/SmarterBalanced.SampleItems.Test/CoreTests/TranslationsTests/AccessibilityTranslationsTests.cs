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

            accessibilityResourceViewModels = new List<AccessibilityResourceViewModel>
            {
                new AccessibilityResourceViewModel()
                {
                    DefaultCode = "TDS_TEST1",
                    SelectedCode = "TDS_TEST1",
                    AccessibilityListItems = accSelectListItems,
                    Label = "Resource 1",
                    Disabled = false
                },
                new AccessibilityResourceViewModel()
                {
                    DefaultCode = "TDS_TEST2",
                    SelectedCode = "TDS_TEST2",
                    AccessibilityListItems = accSelectListItems,
                    Label = "Resource 2",
                    Disabled = false
                }
            };
        }

        #region ToIsaap

        /// <summary>
        /// Tests happy case AccessibilityResourceViewModels to ISAAP code
        /// </summary>
        [Fact]
        public void TestItemsToISAAP()
        {
            string isaap = AccessibilityTranslations.ToISAAP(accessibilityResourceViewModels);
            string expectedIsaap = $"{accessibilityResourceViewModels[0].SelectedCode};{accessibilityResourceViewModels[1].SelectedCode}";
            Assert.Equal(expectedIsaap, isaap);
        }

        /// <summary>
        /// Tests null AccessibilityResourceViewModels to ISAAP code
        /// </summary>
        [Fact]
        public void TestNullItemsToISAAP()
        {
            Assert.Throws<ArgumentNullException>(() => AccessibilityTranslations.ToISAAP(null));
        }

        /// <summary>
        /// Tests empty AccessibilityResourceViewModels to ISAAP code
        /// </summary>
        [Fact]
        public void TestEmptyItemsToISAAP()
        {
            string isaap = AccessibilityTranslations.ToISAAP(new List<AccessibilityResourceViewModel>());
            Assert.Equal("", isaap);
        }

        #endregion

        #region ToAccessibilityResourceViewModels with ISAAP

        /// <summary>
        /// Tests happy case accessibility resource translation to view model with isaap
        /// </summary>
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

        /// <summary>
        /// Tests accessibility resource translation to view model with nonexistent isaap
        /// </summary>
        [Fact]
        public void TestAccessibilityResourceToViewModelWithBadISAAP()
        {
            AccessibilityResource accResource = accessibilityResources[0];
            List<AccessibilityResource> accResources = new List<AccessibilityResource>
            {
                accResource
            };
            string ISAAP = "NOT_THERE;";
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accResources, ISAAP);

            Assert.Equal(accResources.Count(), accResourceViewModels.Count());

            AccessibilityResourceViewModel accResourceViewModel = accResourceViewModels[0];

            Assert.Equal(null, accResourceViewModel.SelectedCode);
            Assert.Equal(accResource.Label, accResourceViewModel.Label);
            Assert.Equal(accResource.Disabled, accResourceViewModel.Disabled);
        }

        /// <summary>
        /// Test happy case multiple accessibility resources with isaap translation to view models
        /// </summary>
        [Fact]
        public void TestAccessibilityResourceToViewModelsWithISAAP()
        {
            string ISAAP = "TDS_TEST3;";
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accessibilityResources, ISAAP);

            Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }

        /// <summary>
        /// Test null accessibility resources with isaap translation to view models
        /// </summary>
        [Fact]
        public void TestNullAccessibilityResourceToViewModelsWithISAAP()
        {
            string ISAAP = "TDS_TEST3;";
        
            Assert.Throws<ArgumentNullException>(() => AccessibilityTranslations
                .ToAccessibilityResourceViewModels(null, ISAAP));
        }

        /// <summary>
        /// Test multiple accessibility resources with null isaap translation to view models
        /// </summary>
        [Fact]
        public void TestAccessibilityResourceToViewModelsWithNullISAAP()
        {
            string ISAAP = null;
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accessibilityResources, ISAAP);

            Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }

        /// <summary>
        /// Test empty accessibility resources with isaap translation to view models
        /// </summary>
        [Fact]
        public void TestEmptyAccessibilityResourceToViewModelsWithISAAP()
        {
            string ISAAP = "TDS_TEST3;";
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(new List<AccessibilityResource>(), ISAAP);

            Assert.Equal(0, accResourceViewModels.Count());
        }

        #endregion

        #region ToAccessibilityResourceViewModels
        /// <summary>
        /// Tests happy case accessibilty resource translation to a respective viewmodel
        /// </summary>
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

        /// <summary>
        /// Tests happy case accessibilty resource translation to a respective viewmodel
        /// where accessibility resource is disabled
        /// </summary>
        [Fact]
        public void TestAccessibilityResourceToViewModelDisabled()
        {
            AccessibilityResource accResource = accessibilityResources[0];
            accResource.Disabled = true;

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


        /// <summary>
        /// Test happy case multiple accessibility resources translation to view models
        /// </summary>
        [Fact]
        public void TestAccessibilityResourcesToViewModels()
        {
            List<AccessibilityResourceViewModel> accResourceViewModels = 
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(accessibilityResources);

            Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }

        /// <summary>
        /// Test empty accessibility resources list translation to view models
        /// </summary>
        [Fact]
        public void TestEmptyAccessibilityResourcesToViewModels()
        {
            List<AccessibilityResourceViewModel> accResourceViewModels =
                AccessibilityTranslations
                .ToAccessibilityResourceViewModels(new List<AccessibilityResource> ());

            Assert.Equal(0, accResourceViewModels.Count());
        }

        /// <summary>
        /// Test null accessibility resource translation to view models
        /// </summary>
        [Fact]
        public void TestNullAccessibilityResourceToViewModels()
        {
            Assert.Throws<ArgumentNullException>(() => AccessibilityTranslations
                .ToAccessibilityResourceViewModels(null));
        }
        #endregion
    }

}

