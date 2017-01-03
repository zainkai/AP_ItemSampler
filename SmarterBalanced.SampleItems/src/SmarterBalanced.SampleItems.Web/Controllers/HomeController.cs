using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Net;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings appsettings;
        public HomeController(AppSettings settings)
        {
            appsettings = settings;
        }

        /// <summary>
        /// Checks useragent string for IE11/Browser compatibility
        /// redirects to BrowserWarning and adds cookie if not
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
       {
            string cookieName = appsettings.SettingsConfig.BrowserWarningCookie;
            var cookie = "true";
            double browser;
            var useragent = Request.Headers["User-Agent"].ToString();
            //Response.Cookies.Delete(cookieName);

            if (useragent.Contains("Trident") && CookieManager.GetCookie(Request, cookieName) == null)
            {
                string regex = appsettings.SettingsConfig.UserAgentRegex;

                if (Regex.IsMatch(useragent, regex))
                {

                    browser = Convert.ToDouble(Regex.Match(useragent, regex).Groups[1].ToString());

                    if (browser <= 7.0)
                    {
                        Response.Cookies.Append(cookieName, cookie);
                        return RedirectToAction("BrowserWarning");
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
            string browser;
            var useragent = Request.Headers["User-Agent"].ToString();

            if (useragent.Contains("Trident/7.0"))
            {
                browser = "Internet Explorer 11.0/ Chrome 55.X / Firefox 50.X";
            }

            else
            {
                browser = "Internet Explorer 10 or an unsupported browser";
            }

            ViewData["ReturnUrl"] = Request.Headers["Referer"].ToString();
            ViewData["BrowserVersion"] = browser;

            return View();
        }
    }
}
