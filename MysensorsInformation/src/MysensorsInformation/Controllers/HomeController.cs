using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNet.Mvc;
using MysensorListener.Settings;
using Microsoft.Extensions.OptionsModel;

namespace MysensorListener.Controllers
{
    public class HomeController : Controller
    {
        private readonly GeneralSettings _generalSettings;
        private readonly NRF24State _nrf24State;
        private readonly MysensorsState _mysensorsState;
        private readonly VeraSettings _veraSettings;

        public HomeController(
            IOptions<GeneralSettings> generalSettings,
            NRF24State nrf24State,
            MysensorsState mysensorsState,
            VeraSettings veraSettings)
        {
            _generalSettings = generalSettings.Value;
            _nrf24State = nrf24State;
            _mysensorsState = mysensorsState;
            _veraSettings = veraSettings;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetSettings()
        {
            var records = new List<object>();

            records.AddRange(HomeController.ProcessObject(_generalSettings));
            records.AddRange(HomeController.ProcessObject(_nrf24State));
            records.AddRange(HomeController.ProcessObject(_mysensorsState));
            records.AddRange(HomeController.ProcessObject(_veraSettings));

            return Json(records);
        }

        private static List<object> ProcessObject(object obj)
        {
            var records = new List<object>();

            foreach (var property in obj.GetType().GetProperties())
            {
                var enumerable = property.GetValue(obj, null) as IEnumerable;
                var value = enumerable != null 
                    ? $"{enumerable?.Cast<object>().Count()} items" 
                    : property.GetValue(obj, null).ToString();

                var attribute = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var record = HomeController.CreateRecord(
                    obj.GetType().Name,
                    property.Name,
                    value,
                    attribute?.Description);
                records.Add(record);
            }

            return records;
        }

        private static object CreateRecord(string group, string name, string value, string description)
        {
            return new
            {
                group,
                name,
                value,
                description
            };
        }
    }
}
