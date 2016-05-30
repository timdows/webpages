using System.Web.Mvc;

namespace DenSGame.Controllers
{
    public class HomeController : DenSGameController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}