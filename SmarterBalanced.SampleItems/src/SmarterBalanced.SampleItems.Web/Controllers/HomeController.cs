using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System.Net;
using System.Linq;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings appsettings;
        public HomeController(AppSettings settings)
        {
            appsettings = settings;
        }


        public IActionResult Index()
        {
            string cookieName = appsettings.SettingsConfig.BrowserWarningCookie;
            var cookie = "true";
            string browser;
            var useragent = Request.Headers["User-Agent"].ToString();

            if (useragent.Contains("Trident"))
            {
                string regex = 'Trident\/(\d.\d)';
                if (System.Text.RegularExpressions.Regex.IsMatch(useragent, regex))
                {
                    RedirectToAction("BrowserWarning");
                }
            }
            //Response.Cookies.Append(cookieName, cookie);
            //Response.Cookies[cookieName]
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

            if((int)HttpStatusCode.BadRequest == code.GetValueOrDefault())
            {
                ViewData["Error"] = "400 Bad Request";
            }

            return View();
        }

        public IActionResult BrowserWarning()
        {
            string browser;
            var useragent = Request.Headers["User-Agent"].ToString();
                        
            if (useragent.Contains("Trident/7.0") || useragent.Contains("Chrome/55.0") || useragent.Contains("Firefox/50"))
            {
                browser = "Internet Explorer 11.0/ Chrome 55.X / Firefox 50.X";
            }
            else
            {
                browser = "Internet Explorer 10 or an unsupported browser";
            }

            ViewData["BrowserVersion"] = browser;
            return View();
        }
    }
}
