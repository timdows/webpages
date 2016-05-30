using System.Web.Mvc;

namespace DenSGame.Controllers.Partials
{
    public class PartialsController : Controller
    {
        public PartialViewResult Header()
        {
            this.ViewBag.LogoTitle = "D&S = 60";
            return PartialView();
        }

        public PartialViewResult Dashboard()
        {
            return PartialView();
        }

        public PartialViewResult SidebarLeft()
        {
            return PartialView();
        }

        public PartialViewResult Name()
        {
            return PartialView();
        }

        public PartialViewResult Scores()
        {
            return PartialView();
        }

        public PartialViewResult CurrentQuestion()
        {
            return PartialView();
        }

        public PartialViewResult AddQuestion()
        {
            return PartialView();
        }

        public PartialViewResult SelectQuestion()
        {
            return PartialView();
        }

        public PartialViewResult SelectWinningAnswer()
        {
            return PartialView();
        }
    }
}