using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        GlobalAccessibilityViewModel globalAccViewModel;
        string accCookieName;

        public GlobalAccessibilityControllerTests()
        {
            string iSSAP = "TDS_test;TDS_test2;";
            accCookieName = "accessibilitycookie";


            globalAccViewModel = new GlobalAccessibilityViewModel()
            {
                AccessibilityResourceViewModels = new List<AccessibilityResourceViewModel>
                {
                    new AccessibilityResourceViewModel()
                    {
                        Label = "Test1",
                        Disabled = false,
                        AccessibilityListItems = new List<SelectListItem>()
                    },
                    new AccessibilityResourceViewModel()
                    {
                        Label = "Test2",
                        Disabled = false,
                        AccessibilityListItems = new List<SelectListItem>()
                    }
                }
            };

            var appSettings = new AppSettings()
            {
                SettingsConfig = new SettingsConfig()
                {
                    AccessibilityCookie = accCookieName
                }
            };

            var globalAccessibilityRepoMock = new Mock<IGlobalAccessibilityRepo>();
            globalAccessibilityRepoMock.Setup(x => x.GetGlobalAccessibilityViewModel(iSSAP)).Returns(globalAccViewModel);
            globalAccessibilityRepoMock.Setup(x => x.GetISSAPCode(globalAccViewModel)).Returns("Test1;Test2;");

            globalAccessibilityRepoMock.Setup(x => x.GetSettings()).Returns(appSettings);

            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Cookies[accCookieName]).Returns(iSSAP);

            var response = new Mock<HttpResponse>();
            response.Setup(x => x.Cookies.Append(accCookieName, iSSAP));
            response.Setup(x => x.Cookies.Delete(accCookieName));

            var context = new Mock<HttpContext>();
            context.Setup(x => x.Request).Returns(request.Object);
            context.Setup(x => x.Response).Returns(response.Object);

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

        [Fact]
        public void TestIndexWithViewModel()
        {
            var result = controller.Index(globalAccViewModel);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }


        [Fact]
        public void TestResetAction()
        {
            var result = controller.Index(globalAccViewModel);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", viewResult.ActionName);
        }


    }
}
