using Moq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.DalTests.TranslationsTests
{
    public class AccessibilityTranslationTests
    {
        public List<AccessibilityResource> Resources { get; set; }
        public List<AccessibilityResource> PartialResources { get; set; }

        public AccessibilityTranslationTests()
        {
            Resources = new List<AccessibilityResource>
            {
                AccessibilityResource.Create(
                    resourceCode: "ACC1",
                    order: 1,
                    disabled: false,
                    defaultSelection: "ACC1_SEL1",
                    label: "Accessibility 1",
                    description: "Accessibility Selection One",
                    selections: ImmutableArray.Create(
                        new AccessibilitySelection(
                            code: "ACC1_SEL1",
                            order: 1,
                            disabled: false,
                            label: "Selection 1"))),
                AccessibilityResource.Create(
                    resourceCode: "ACC2",
                    order: 2,
                    disabled: false,
                    defaultSelection: "ACC2_SEL2",
                    label: "Accessibility 2",
                    description: "Accessibility Selection Two",
                    selections: ImmutableArray.Create(
                        new AccessibilitySelection(
                            code: "ACC2_SEL1",
                            order: 1,
                            disabled: false,
                            label: "Selection 1"),
                        new AccessibilitySelection(
                            code: "ACC2_SEL2",
                            order: 2,
                            disabled: false,
                            label: "Selection 2")))
            };

            PartialResources = new List<AccessibilityResource>
            {
                AccessibilityResource.Create(
                    resourceCode: "ACC1",
                    selections: ImmutableArray.Create(
                        AccessibilitySelection.Create(
                            code: "ACC1_SEL1",
                            label: "Selection 1"))),
                AccessibilityResource.Create(
                    resourceCode: "ACC2",
                    selections: ImmutableArray.Create(
                        AccessibilitySelection.Create(
                            code: "ACC1_SEL1",
                            label: "Selection 1"),
                        AccessibilitySelection.Create(
                            code: "ACC1_SEL2",
                            label: "Selection 2"))),
            };
        }
      
        #region ToAccessibilityResourceTests
        /// <summary>
        /// Tests exception thrown if ToAccessibilityResource is given a 
        /// null partial resource
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourceNullPartialAccessibility()
        {
            AccessibilityResource r = null;
            Assert.Throws<ArgumentNullException>(() => r.MergeWith(Resources[0]));
        }

        /// <summary>
        /// Tests exception thrown if ToAccessibilityResource is given a 
        /// null global resource
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourceNullGlobalAccessibility()
        {
            AccessibilityResource r = AccessibilityResource.Create();
            Assert.Throws<ArgumentNullException>(() => r.MergeWith(null));
        }

        /// <summary>
        /// Tests that a global accessibility resource copy is not modified 
        /// from the original given a partial accessibility resource that 
        /// contains all of the select elements
        /// </summary>
        [Fact] public void TestToAccessibilityResourceNoChanges()
        {
            AccessibilityResource globalResource = Resources[1];
            AccessibilityResource partialResource = AccessibilityResource.Create(
                resourceCode: "ACC1",
                selections: ImmutableArray.Create(
                    AccessibilitySelection.Create(code: "ACC2_SEL1"),
                    AccessibilitySelection.Create(code: "ACC2_SEL2")));

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .MergeWith(partialResource, globalResource);

            Assert.Equal(globalResource.CurrentSelectionCode, outputResource.CurrentSelectionCode);
            Assert.Equal(globalResource.Disabled, outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);
            Assert.Equal(globalResource.DefaultSelection, outputResource.DefaultSelection);
            Assert.Equal(globalResource.Selections.Count(), outputResource.Selections.Count());
        }

        /// <summary>
        /// Tests that a global accessibility resource copy is 
        /// only disabled given a partial accessibility resource 
        /// that contains all of the select elements
        /// </summary>
        [Fact]
        public void TestToAccessibilityDisabledResource()
        {
            AccessibilityResource globalResource = Resources[1];
            AccessibilityResource partialResource = AccessibilityResource.Create(
                resourceCode: "ACC2",
                disabled: true,
                selections: ImmutableArray.Create(
                    AccessibilitySelection.Create(code: "ACC2_SEL1"),
                    AccessibilitySelection.Create(code: "ACC2_SEL2")));

            AccessibilityResource outputResource = partialResource.MergeWith(globalResource);

            Assert.Equal(globalResource.CurrentSelectionCode, outputResource.CurrentSelectionCode);
            Assert.True(outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);
            Assert.Equal(string.Empty, outputResource.DefaultSelection);
            Assert.Equal(globalResource.Selections.Length, outputResource.Selections.Length);

            foreach (var sel in outputResource.Selections)
            {
                Assert.True(sel.Disabled);
            }
        }


        /// <summary>
        /// Tests that a global accessibility resource copy's selections 
        /// are disabled given a partial accessibility resource that does not 
        /// contain any select elements
        /// </summary>
        [Fact]
        public void TestToAccessibilityDisabledAllSelections()
        {
            AccessibilityResource globalResource = Resources[1];
            AccessibilityFamilyResource partialResource = new AccessibilityFamilyResource(
            resourceCode: "ACC2",
            selections: ImmutableArray<AccessibilityFamilySelection>.Empty,
            disabled: false);

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .MergeGlobalResource(partialResource, globalResource);

            Assert.Equal(globalResource.CurrentSelectionCode, outputResource.CurrentSelectionCode);
            Assert.Equal(false, outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);
            Assert.Equal(string.Empty, outputResource.DefaultSelection);
            Assert.Equal(globalResource.Selections.Count(), outputResource.Selections.Count());
            foreach (var sel in outputResource.Selections)
            {
                Assert.Equal(true, sel.Disabled);
            }
        }

        /// <summary>
        /// Tests that a some of a global accessibility resource copy's 
        /// selections are disabled given a partial accessibility resource 
        /// that only contains one select element. Also check that default
        /// AccessibilityResource selection is updated
        /// </summary>
        [Fact]
        public void TestToAccessibilityDisabledSomeSelections()
        {
            AccessibilityResource globalResource = Resources[1];
            AccessibilityFamilyResource partialResource = new AccessibilityFamilyResource(
            resourceCode: "TDS_CC",
            disabled: false,
            selections: ImmutableArray.Create(AccessibilityFamilySelection.Create(code: "ACC2_SEL1")));

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .MergeGlobalResource(partialResource, globalResource);

            Assert.Equal(globalResource.CurrentSelectionCode, outputResource.CurrentSelectionCode);
            Assert.Equal(false, outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);

            // Check that default selection was also updated
            Assert.Equal("ACC2_SEL1", outputResource.DefaultSelection); 

            Assert.Equal(globalResource.Selections.Length, outputResource.Selections.Length);
            Assert.Equal(false, outputResource.Selections[0].Disabled);
            Assert.Equal(true, outputResource.Selections[1].Disabled);
        }

        #endregion

        #region ToAccessibilityResourcesTests
        /// <summary>
        /// Tests translation of AccessibilityResources given a family's 
        /// resources, where the resources are not in the family's resources.
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourcesNotModified()
        {
            List<AccessibilityResource> noPartialResources = new List<AccessibilityResource>();
            var resultResources = AccessibilityResourceTranslation.MergeAllWith(noPartialResources, Resources);

            Assert.Equal(Resources.Count, resultResources.Length);
        }


        /// <summary>
        /// Tests translation of AccessibilityResource given a family's 
        /// resources, where the resource is not in the family's resources.
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourceNotModified()
        {
            List<AccessibilityResource> noPartialResources = new List<AccessibilityResource>();
            AccessibilityResource inputResource = Resources[1];
            List<AccessibilityResource> inputResources = new List<AccessibilityResource>
            {
                inputResource
            };

            var resultResources = AccessibilityResourceTranslation.MergeAllWith(noPartialResources, inputResources);
            Assert.Equal(inputResources.Count, resultResources.Length);

            AccessibilityResource outputResource = resultResources[0];

            Assert.Equal(inputResource.CurrentSelectionCode, outputResource.CurrentSelectionCode);
            Assert.Equal(inputResource.Description, outputResource.Description);
            Assert.Equal(inputResource.Disabled, outputResource.Disabled);
            Assert.Equal(inputResource.DefaultSelection, outputResource.DefaultSelection);
            Assert.Equal(inputResource.Order, outputResource.Order);
            Assert.Equal(inputResource.Selections.Count(), outputResource.Selections.Count());
        }

        /// <summary>
        /// Tests that global resources are passed through an empty list
        /// of partial resources in mergeAllWith()
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourcesMatchingPartialResource()
        {
            var resultResources = AccessibilityResourceTranslation.MergeAllWith(new List<AccessibilityResource>(), Resources);
            Assert.Equal(Resources.Count, resultResources.Length);
        }
        #endregion

        /* TODO:
         * This did not test to see if there was a family resource. We changed the select statement in MergeAllWith
         * to Code. This should have broken a test.
         * */
    }
}
