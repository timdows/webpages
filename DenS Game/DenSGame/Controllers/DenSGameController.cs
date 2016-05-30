using System.Data.Entity;
using System.Web.Mvc;

namespace DenSGame.Controllers
{
    public abstract class DenSGameController : Controller
    {
        public new JsonResult Json(object data)
        {
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    public abstract class DenSGameController<TDataContext> : DenSGameController
        where TDataContext : DbContext, new()
    {
        protected readonly TDataContext _dataContext;

        protected DenSGameController()
        {
            _dataContext = new TDataContext();
        }

        protected DenSGameController(TDataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}