using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using Microsoft.Extensions.Logging;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings appsettings;
        private readonly ILogger logger;
        public HomeController(AppSettings settings, ILoggerFactory loggerFactory)
        {
            appsettings = settings;
            logger = loggerFactory.CreateLogger<HomeController>();
        }
        /// <summary>
        /// Checks useragent string for IE11/Browser compatibility
        /// redirects to BrowserWarning and adds cookie if not
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string cookieName = appsettings.SettingsConfig.BrowserWarningCookie;
            var useragent = Request.Headers["User-Agent"].ToString();
            if (CookieManager.GetCookie(Request, cookieName) == null && useragent.Contains("Trident"))
            {
                string regex = appsettings.SettingsConfig.UserAgentRegex;
                Match match = Regex.Match(useragent, regex);
                if (match.Success)
                {
                    logger.LogInformation($"Using Internet Explorer, UserAgent: {useragent}");
                    double browser;
                    double.TryParse(match.Groups[1]?.Value, out browser);
                    if (browser < 7.0)
                    {
                        Response.Cookies.Append(cookieName, cookieName);
                        return RedirectToAction(nameof(BrowserWarning));
                    }
                }
            }
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult StatusCodeError(int? code)
        {
            ViewData["Error"] = "Something went wrong";

            if ((int)HttpStatusCode.BadRequest == code.GetValueOrDefault())
            {
                ViewData["Error"] = "400 Bad Request";
            }

            return View();
        }

        public IActionResult BrowserWarning()
        {
            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            return View();
        }
    }
}