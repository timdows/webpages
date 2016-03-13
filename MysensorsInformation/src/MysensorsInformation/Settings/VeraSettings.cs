using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.OptionsModel;
using MysensorListener.Models;
using Newtonsoft.Json.Linq;

namespace MysensorListener.Settings
{
    public class VeraSettings
    {
        private readonly GeneralSettings _generalSettings;

        public VeraSettings(IOptions<GeneralSettings> generalSettings)
        {
            _generalSettings = generalSettings.Value;
            Task.Run(() => GetSettings()).Wait();
        }

        private async void GetSettings()
        {
            this.VeraRooms = new List<VeraRoom>();
            this.VeraDevices = new List<VeraDevice>();

            using (var webClient = new HttpClient())
            {
                var result = await webClient.GetStringAsync(
                    $"http://{_generalSettings.VeraIpAddress}/port_3480/data_request?id=user_data&output_format=json");
                var json = JObject.Parse(result);

                foreach (var room in json["rooms"])
                {
                    this.VeraRooms.Add(new VeraRoom
                    {
                        ID = room["id"].ToObject<long>(),
                        Name = room["name"].ToString()
                    });
                }

                // Get the gateway
                VeraDevice gateway = null;
                foreach (var device in json["devices"])
                {
                    if (device["device_type"] == null || device["ip"] == null)
                        continue;

                    // Based on device_type and the ip configuration
                    if (device["device_type"].ToString() == "urn:schemas-arduino-cc:device:arduino:1" &&
                       device["ip"].ToString() == $"{_generalSettings.MysensorsIpAddress}:{_generalSettings.MysensorsPort}")
                    {
                        gateway = CreateVeraDevice(device, true);
                        this.VeraDevices.Add(gateway);
                    }
                }

                if (gateway == null)
                    return;

                // Get all the arduino mysensor devices
                foreach (var device in json["devices"])
                {
                    // It should be a child of the gateway
                    if (device["id_parent"] == null || device["id_parent"].ToObject<long>() != gateway.ID)
                        continue;

                    this.VeraDevices.Add(CreateVeraDevice(device, false));
                }
            }
        }

        private VeraDevice CreateVeraDevice(JToken device, bool isGateway)
        {
            var id = long.Parse(device["id"].ToString());
            var parentID = device["id_parent"].ToObject<long>();
            var name = device["name"].ToString();
            var altID = device["altid"].ToString();
            var room = long.Parse(device["room"].ToString());
            var veraRoom = this.VeraRooms.SingleOrDefault(a_item => a_item.ID == room);

            return new VeraDevice
            {
                ID = id,
                ParentID = parentID,
                Name = name,
                AltID = altID,
                VeraRoom = veraRoom,
                IsGateway = isGateway
            };
        }

        [Description("Holds a list with all the VeraRooms")]
        public List<VeraRoom> VeraRooms { get; private set; }
        [Description("Holds a list with all the VeraDevices")]
        public List<VeraDevice> VeraDevices { get; private set; }
    }
}
