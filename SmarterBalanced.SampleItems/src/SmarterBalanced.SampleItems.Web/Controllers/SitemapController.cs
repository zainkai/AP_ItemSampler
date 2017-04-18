using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleMvcSitemap;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class SitemapController : Controller
    {
        public IActionResult Index()
        {
            List<SitemapNode> nodes = new List<SitemapNode>
            {
                new SitemapNode(Url.Action("Index","Home")),
                new SitemapNode(Url.Action("Index","BrowseItems")),
                new SitemapNode(Url.Action("Index","AboutItems"))
             };

            return new SitemapProvider().CreateSitemap(new SitemapModel(nodes));
        }
    }
}
