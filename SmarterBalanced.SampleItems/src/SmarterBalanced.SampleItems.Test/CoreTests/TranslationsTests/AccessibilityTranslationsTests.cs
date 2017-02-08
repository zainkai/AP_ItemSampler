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

        #region ToIsaap

        /// <summary>
        /// Tests happy case AccessibilityResourceViewModels to ISAAP code
        /// </summary>
        [Fact(Skip ="TODO")]
        public void TestItemsToISAAP()
        {
            //string isaap = AccessibilityTranslations.ToISAAP(accessibilityResourceViewModels);
            //string expectedIsaap = $"{accessibilityResourceViewModels[0].SelectedCode};{accessibilityResourceViewModels[1].SelectedCode}";
            //Assert.Equal(expectedIsaap, isaap);
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
        [Fact(Skip = "TODO")]
        public void TestEmptyItemsToISAAP()
        {
            //string isaap = AccessibilityTranslations.ToISAAP(new List<AccessibilityResourceViewModel>());
            //Assert.Equal("", isaap);
        }

        #endregion

        #region ToAccessibilityResourceViewModels with ISAAP

        /// <summary>
        /// Tests happy case accessibility resource translation to view model with isaap
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestAccessibilityResourceToViewModelWithISAAP()
        {
            //AccessibilityResource accResource = accessibilityResources[0];
            //List<AccessibilityResource> accResources = new List<AccessibilityResource>
            //{
            //    accResource
            //};
            //string[] ISAAP = { "TDS_TEST3" };
            //var expectedCode = "TDS_TEST3";
            //List<AccessibilityResourceViewModel> accResourceViewModels =
            //    AccessibilityTranslations
            //    .ToAccessibilityResourceViewModels(accResources, ISAAP);

            //Assert.Equal(accResources.Count(), accResourceViewModels.Count());

            //AccessibilityResourceViewModel accResourceViewModel = accResourceViewModels[0];

            //Assert.Equal(expectedCode, accResourceViewModel.SelectedCode);
            //Assert.Equal(accResource.Label, accResourceViewModel.Label);
            //Assert.Equal(accResource.Disabled, accResourceViewModel.Disabled);
        }

        /// <summary>
        /// Tests accessibility resource translation to view model with nonexistent isaap
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestAccessibilityResourceToViewModelWithBadISAAP()
        {
            //AccessibilityResource accResource = accessibilityResources[0];
            //List<AccessibilityResource> accResources = new List<AccessibilityResource>
            //{
            //    accResource
            //};
            //string[] ISAAP = { "NOT_THERE" };
            //List<AccessibilityResourceViewModel> accResourceViewModels =
            //    AccessibilityTranslations
            //    .ToAccessibilityResourceViewModels(accResources, ISAAP);

            //Assert.Equal(accResources.Count(), accResourceViewModels.Count());

            //AccessibilityResourceViewModel accResourceViewModel = accResourceViewModels[0];

            //Assert.Equal(null, accResourceViewModel.SelectedCode);
            //Assert.Equal(accResource.Label, accResourceViewModel.Label);
            //Assert.Equal(accResource.Disabled, accResourceViewModel.Disabled);
        }

        /// <summary>
        /// Test happy case multiple accessibility resources with isaap translation to view models
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestAccessibilityResourceToViewModelsWithISAAP()
        {
            //string[] ISAAP = { "TDS_TEST3" };
            //List<AccessibilityResourceViewModel> accResourceViewModels =
            //    AccessibilityTranslations
            //    .ToAccessibilityResourceViewModels(accessibilityResources, ISAAP);

            //Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }

        /// <summary>
        /// Test null accessibility resources with isaap translation to view models
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestNullAccessibilityResourceToViewModelsWithISAAP()
        {
            //string[] ISAAP = { "TDS_TEST3" };
        
            //Assert.Throws<ArgumentNullException>(() => AccessibilityTranslations
            //    .ToAccessibilityResourceViewModels(null, ISAAP));
        }

        /// <summary>
        /// Test multiple accessibility resources with null isaap translation to view models
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestAccessibilityResourceToViewModelsWithEmptyISAAP()
        {
            //string[] ISAAP = new string[0];
            //List<AccessibilityResourceViewModel> accResourceViewModels =
            //    AccessibilityTranslations
            //    .ToAccessibilityResourceViewModels(accessibilityResources, ISAAP);

            //Assert.Equal(accessibilityResources.Count(), accResourceViewModels.Count());
        }

        /// <summary>
        /// Test empty accessibility resources with isaap translation to view models
        /// </summary>
        [Fact(Skip = "TODO")]
        public void TestEmptyAccessibilityResourceToViewModelsWithISAAP()
        {
            //string[] ISAAP = { "TDS_TEST3" };
            //List<AccessibilityResourceViewModel> accResourceViewModels =
            //    AccessibilityTranslations
            //    .ToAccessibilityResourceViewModels(new List<AccessibilityResource>(), ISAAP);

            //Assert.Equal(0, accResourceViewModels.Count());
        }

        #endregion
    }

}

