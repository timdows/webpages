using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNet.Mvc;
using MysensorListener.Settings;
using Microsoft.Extensions.OptionsModel;
using MysensorListener.Models;

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
            var records = new List<SettingsDTO>();

            records.AddRange(HomeController.ProcessObject(_generalSettings, true, false));
            records.AddRange(HomeController.ProcessObject(_nrf24State, false, true));
            records.AddRange(HomeController.ProcessObject(_mysensorsState, false, true));
            records.AddRange(HomeController.ProcessObject(_veraSettings, false, true));

            return Json(records);
        }

        public JsonResult PostSettings([FromBody] SettingsDTO settingsDTO)
        {
            if (!(settingsDTO.Editable && settingsDTO.Group == "GeneralSettings"))
                return Json(false);
            
            //TODO change to reflexion
            //object obj = typeof(GeneralSettings).GetProperty(settingsDTO.Name);
            //obj = settingsDTO.Value;
            //_generalSettings.MysensorsIpAddress = settingsDTO.Value;

            switch(settingsDTO.Name)
            {
                case "VeraIpAddress":
                    _generalSettings.VeraIpAddress = settingsDTO.Value;
                    break;
                case "MysensorsIpAddress":
                    _generalSettings.MysensorsIpAddress = settingsDTO.Value;
                    break;
                case "MysensorsPort":
                    _generalSettings.MysensorsPort = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "PortName":
                    _generalSettings.PortName = settingsDTO.Value;
                    break;
                case "BaudRate":
                    _generalSettings.BaudRate = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "RfChannel":
                    _generalSettings.RfChannel = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "DataRate":
                    _generalSettings.DataRate = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "AddressLength":
                    _generalSettings.AddressLength = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "BaseAddress":
                    _generalSettings.BaseAddress = settingsDTO.Value;
                    break;
                case "CrcLength":
                    _generalSettings.CrcLength = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "MaximumPayloadSize":
                    _generalSettings.MaximumPayloadSize = Convert.ToInt32(settingsDTO.Value);
                    break;
                case "LookupMysensorsNodeViaVera":
                    _generalSettings.LookupMysensorsNodeViaVera = Convert.ToBoolean(settingsDTO.Value);
                    break;
            }

            _nrf24State.RequestUploadConfiguration = true;

            return Json(true);
        }

        private static List<SettingsDTO> ProcessObject(object obj, bool editable, bool useEnumerable)
        {
            var records = new List<SettingsDTO>();

            foreach (var property in obj.GetType().GetProperties())
            {
                var enumerable = property.GetValue(obj, null) as IEnumerable;
                var value = enumerable != null && useEnumerable
                    ? $"{enumerable?.Cast<object>().Count()} items" 
                    : property.GetValue(obj, null).ToString();

                var attribute = Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute;
                var record = HomeController.CreateRecord(
                    obj.GetType().Name,
                    property.Name,
                    value,
                    attribute?.Description,
                    editable);
                records.Add(record);
            }

            return records;
        }

        private static SettingsDTO CreateRecord(string group, string name, string value, string description, bool editable)
        {
            return new SettingsDTO
            {
                Group = group,
                Name = name,
                Value = value,
                Description = description,
                Editable = editable
            };
        }
    }
}
