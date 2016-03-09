using System.Threading;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Extensions.OptionsModel;
using MysensorListener.Settings;

namespace MysensorListener.Controllers.NRF24
{
    public class NRF24Controller : Controller
    {
        public NRF24Controller(
            IConnectionManager connectionManager,
            IOptions<GeneralSettings> generalSettings,
            NRF24State nrf24State,
            VeraSettings veraSettings)
        {
            // Make sure the serial capture process is only running once
            if (nrf24State.Started)
                return;

            nrf24State.Started = true;

            var hub = new NRF24Hub(connectionManager, generalSettings.Value, nrf24State, veraSettings);
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                hub.StartSerialClient();
            }).Start();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
