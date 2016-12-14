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
    }
}
