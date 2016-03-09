using System.Threading;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Extensions.OptionsModel;
using MysensorListener.Settings;

namespace MysensorListener.Controllers.Mysensors
{
    public class MysensorsController : Controller
    {

        public MysensorsController(
            IConnectionManager connectionManager, 
            IOptions<GeneralSettings> generalSettings, 
            VeraSettings veraSettings,
            MysensorsState mysensorsState)
        {
            // Make sure the process is only running once
            if (mysensorsState.Started)
                return;

            mysensorsState.Started = true;

            var hub = new MysensorsHub(connectionManager, veraSettings, generalSettings.Value, mysensorsState);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                hub.StartTelnetClient();
            }).Start();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
