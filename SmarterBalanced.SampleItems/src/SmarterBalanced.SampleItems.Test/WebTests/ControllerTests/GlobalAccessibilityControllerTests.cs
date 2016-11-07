using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Moq;
using SmarterBalanced.SampleItems.Core.Repos;
using SmarterBalanced.SampleItems.Core.Repos.Models;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SmarterBalanced.SampleItems.Test.WebTests.ControllerTests
{
    public class GlobalAccessibilityControllerTests
    {
        GlobalAccessibilityController controller;

        public GlobalAccessibilityControllerTests()
        {
            string ISSAP = "TDS_test;TDS_test2;";
            string accCookieName = "accessibilitycookie";
            var globalAccViewModel = new GlobalAccessibilityViewModel()
            {
                AccessibilityResourceViewModels = new List<AccessibilityResourceViewModel>()
            };

            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
                {
                    AccessibilityCookie = accCookieName
                }
            };

            var globalAccessibilityRepoMock = new Mock<IGlobalAccessibilityRepo>();
            globalAccessibilityRepoMock.Setup(x => x.GetGlobalAccessibilityViewModel(ISSAP)).Returns(globalAccViewModel);

            globalAccessibilityRepoMock.Setup(x => x.GetSettings()).Returns(appSettings);

            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Cookies[accCookieName]).Returns(ISSAP);

            var context = new Mock<HttpContext>();
            context.Setup(x => x.Request).Returns(request.Object);

            var actionDesctiptor = new ControllerActionDescriptor();
            var actionContext = new ActionContext(context.Object, new RouteData(), actionDesctiptor);

            controller = new GlobalAccessibilityController(globalAccessibilityRepoMock.Object);
            controller.ControllerContext = new ControllerContext(actionContext);
        }

        [Fact]
        public void TestIndexDefault()
        {
            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<GlobalAccessibilityViewModel>(viewResult.ViewData.Model);
        }


    }
}
