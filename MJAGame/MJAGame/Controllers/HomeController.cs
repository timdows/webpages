using System.Web.Mvc;

namespace MJAGame.Controllers
{
    public class HomeController : MJAGameController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}