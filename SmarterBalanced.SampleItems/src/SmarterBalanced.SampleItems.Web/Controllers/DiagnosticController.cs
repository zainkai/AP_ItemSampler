using Microsoft.AspNetCore.Mvc;
using SmarterBalanced.SampleItems.Core.Diagnostics;
using System.Threading.Tasks;

namespace SmarterBalanced.SampleItems.Web.Controllers
{
    public class DiagnosticController : Controller
    {

        private IDiagnosticManager diagnosticManager;

        public DiagnosticController(IDiagnosticManager manager)
        {
            diagnosticManager = manager;
        }

        public async Task<IActionResult> Index(int level = 0)
        {
            var xmlString = await diagnosticManager.GetDiagnosticStatusesAsync(level);

            return new ContentResult {
                ContentType = "text/xml",
                Content = xmlString
            };
        }

        public async Task<IActionResult> StatusLocal(int level = 0)
        {
            var xmlString = await diagnosticManager.GetDiagnosticStatusAsync(level);

            return new ContentResult
            {
                ContentType = "text/xml",
                Content = xmlString
            };
        }

    }
  
}
