using Microsoft.AspNetCore.Mvc;

namespace Angular2.Controllers.About
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Created at the AboutController";
            return View();
        }
    }
}
