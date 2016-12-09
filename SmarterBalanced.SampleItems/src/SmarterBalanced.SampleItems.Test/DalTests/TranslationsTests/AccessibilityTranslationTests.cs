using Moq;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;
using System.Collections.Generic;
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
                new AccessibilityResource
                {
                    Code = "ACC1",
                    Order = 1,
                    Disabled = false,
                    DefaultSelection = "ACC1_SEL1",
                    Label = "Accessibility 1",
                    Description = "Accessibility Selection One",
                    Selections = new List<AccessibilitySelection>
                    {
                        new AccessibilitySelection
                        {
                            Code = "ACC1_SEL1",
                            Order = 1,
                            Disabled = false,
                            Label = "Selection 1"
                        }
                    }
                },
                new AccessibilityResource
                {
                    Code = "ACC2",
                    Order = 2,
                    Disabled = false,
                    DefaultSelection = "ACC2_SEL2",
                    Label = "Accessibility 2",
                    Description = "Accessibility Selection Two",
                    Selections = new List<AccessibilitySelection>
                    {
                        new AccessibilitySelection
                        {
                            Code = "ACC2_SEL1",
                            Order = 1,
                            Disabled = false,
                            Label = "Selection 1"
                        },
                        new AccessibilitySelection
                        {
                            Code = "ACC2_SEL2",
                            Order = 2,
                            Disabled = false,
                            Label = "Selection 2"
                        }
                    }
                },
            };

            PartialResources = new List<AccessibilityResource>
            {
                new AccessibilityResource
                {
                    Code = "ACC1",
                    Selections = new List<AccessibilitySelection>
                    {
                        new AccessibilitySelection
                        {
                            Code = "ACC1_SEL1",
                            Label = "Selection 1"
                        }
                    }
                },
                new AccessibilityResource
                {
                    Code = "ACC2",
                    Selections = new List<AccessibilitySelection>
                    {
                        new AccessibilitySelection
                        {
                            Code = "ACC2_SEL1",
                            Label = "Selection 1"
                        },
                        new AccessibilitySelection
                        {
                            Code = "ACC2_SEL2",
                            Label = "Selection 2"
                        }
                    }
                },
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
            AccessibilityResource r = new AccessibilityResource();
            r = null;
            Assert.Throws<ArgumentNullException>(() => r.ToAccessibilityResource(Resources[0]));
        }

        /// <summary>
        /// Tests exception thrown if ToAccessibilityResource is given a 
        /// null global resource
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourceNullGlobalAccessibility()
        {
            AccessibilityResource r = new AccessibilityResource();
            Assert.Throws<ArgumentNullException>(() => r.ToAccessibilityResource(null));
        }

        /// <summary>
        /// Tests that a global accessibility resource copy is not modified 
        /// from the original given a partial accessibility resource that 
        /// contains all of the select elements
        /// </summary>
        [Fact] public void TestToAccessibilityResourceNoChanges()
        {
            AccessibilityResource globalResource = Resources[1];
            AccessibilityResource partialResource = new AccessibilityResource
            {
                Code = "ACC1",
                Selections = new List<AccessibilitySelection>
                {
                    new AccessibilitySelection
                    {
                        Code = "ACC2_SEL1"
                    },
                    new AccessibilitySelection
                    {
                        Code = "ACC2_SEL2"
                    }
                }
            };

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .ToAccessibilityResource(partialResource, globalResource);

            Assert.Equal(globalResource.Code, outputResource.Code);
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
            AccessibilityResource partialResource = new AccessibilityResource
            {
                Code = "ACC1",
                Disabled = true,
                Selections = new List<AccessibilitySelection>
                {
                    new AccessibilitySelection
                    {
                        Code = "ACC1_SEL1"
                    },
                    new AccessibilitySelection
                    {
                        Code = "ACC1_SEL2"
                    }
                }
            };

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .ToAccessibilityResource(partialResource, globalResource);

            Assert.Equal(globalResource.Code, outputResource.Code);
            Assert.Equal(true, outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);
            Assert.Equal(globalResource.DefaultSelection, outputResource.DefaultSelection);
            Assert.Equal(globalResource.Selections.Count(), outputResource.Selections.Count());
            foreach (var sel in outputResource.Selections)
            {
                Assert.Equal(true, sel.Disabled);
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
            AccessibilityResource partialResource = new AccessibilityResource
            {
                Code = "ACC2",
                Selections = null
            };

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .ToAccessibilityResource(partialResource, globalResource);

            Assert.Equal(globalResource.Code, outputResource.Code);
            Assert.Equal(false, outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);
            Assert.Equal(null, outputResource.DefaultSelection);
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
            AccessibilityResource partialResource = new AccessibilityResource
            {
                Code = "ACC2",
                Selections = new List<AccessibilitySelection>
                {
                    new AccessibilitySelection
                    {
                        Code = "ACC2_SEL1"
                    }
                }
            };

            AccessibilityResource outputResource = AccessibilityResourceTranslation
                .ToAccessibilityResource(partialResource, globalResource);

            Assert.Equal(globalResource.Code, outputResource.Code);
            Assert.Equal(false, outputResource.Disabled);
            Assert.Equal(globalResource.Label, outputResource.Label);

            // Check that default selection was also updated
            Assert.Equal("ACC2_SEL1", outputResource.DefaultSelection); 

            Assert.Equal(globalResource.Selections.Count(), outputResource.Selections.Count());
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
            List<AccessibilityResource> resultResources = AccessibilityResourceTranslation.ToAccessibilityResources(noPartialResources, Resources);

            Assert.Equal(Resources.Count(), resultResources.Count());
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

            List<AccessibilityResource> resultResources = AccessibilityResourceTranslation.ToAccessibilityResources(noPartialResources, inputResources);
            Assert.Equal(inputResources.Count(), resultResources.Count());

            AccessibilityResource outputResource = resultResources[0];

            Assert.Equal(inputResource.Code, outputResource.Code);
            Assert.Equal(inputResource.Description, outputResource.Description);
            Assert.Equal(inputResource.Disabled, outputResource.Disabled);
            Assert.Equal(inputResource.DefaultSelection, outputResource.DefaultSelection);
            Assert.Equal(inputResource.Order, outputResource.Order);
            Assert.Equal(inputResource.Selections.Count(), outputResource.Selections.Count());
        }

        /// <summary>
        /// Tests that a global resource is matched with a family's resource
        /// if the codes match
        /// </summary>
        [Fact]
        public void TestToAccessibilityResourcesMatchingPartialResource()
        {
            List<AccessibilityResource> partialResources = new List<AccessibilityResource>
            {
                new AccessibilityResource
                {
                    Code = "ACC2",
                    Selections = new List<AccessibilitySelection>
                    {
                        new AccessibilitySelection
                        {
                            Code = "ACC2_SEL1"
                        }
                   }
                }
            };

            List<AccessibilityResource> resultResources = AccessibilityResourceTranslation.ToAccessibilityResources(partialResources, Resources);

            Assert.Equal(Resources.Count(), resultResources.Count());
        }
        #endregion

 
    }
}
