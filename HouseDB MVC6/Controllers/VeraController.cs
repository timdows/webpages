using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using HouseDB.ClientModels;

namespace HouseDB.Controllers
{
    [Route("api/[controller]")]
    public class VeraController : Controller
    {
        [HttpGet]
        public async Task<JsonResult> Index()
        {
            var clientModel = new SevenSegmentClientModel();
            await clientModel.Load();
            return Json(clientModel);
        }
    }
}
