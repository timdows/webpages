using System.Data.Entity;
using System.Web.Mvc;

namespace MJAGame.Controllers
{
    public abstract class MJAGameController : Controller
    {
        public new JsonResult Json(object data)
        {
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    public abstract class MJAGameController<TDataContext> : MJAGameController
        where TDataContext : DbContext, new()
    {
        protected readonly TDataContext _dataContext;

        protected MJAGameController()
        {
            _dataContext = new TDataContext();
        }

        protected MJAGameController(TDataContext dataContext)
        {
            _dataContext = dataContext;
        }
    }
}