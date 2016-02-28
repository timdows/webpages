using System.Threading;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Extensions.OptionsModel;
using MysensorListener.Settings;

namespace MysensorListener.Controllers
{
    public class MysensorsController : Controller
    {
        private readonly IConnectionManager _connectionManager;
        private readonly GeneralSettings _generalSettings;
        private readonly VeraSettings _veraSettings;

        public MysensorsController(
            IConnectionManager connectionManager, 
            IOptions<GeneralSettings> generalSettings, 
            VeraSettings veraSettings)
        {
            _connectionManager = connectionManager;
            _generalSettings = generalSettings.Value;
            _veraSettings = veraSettings;
        }

        public IActionResult Index()
        {
            var hub = new MysensorsHub(_connectionManager, _veraSettings, _generalSettings);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                hub.StartTelnetClient();
            }).Start();

            return View();
        }
    }
}
