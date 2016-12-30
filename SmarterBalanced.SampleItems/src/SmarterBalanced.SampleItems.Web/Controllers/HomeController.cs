using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
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
