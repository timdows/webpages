using Microsoft.AspNetCore.Mvc;

namespace Angular2.Controllers.Projects
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            ViewData["Message"] = "Created at the ProjectsController";
            return View();
        }
    }
}
