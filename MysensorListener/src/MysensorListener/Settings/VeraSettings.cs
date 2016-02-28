using System.Collections.Generic;
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

                foreach (var device in json["devices"])
                {
                    if (device["id_parent"] == null)
                        continue;

                    var id = long.Parse(device["id"].ToString());
                    var parentID = device["id_parent"].ToObject<long>();
                    var name = device["name"].ToString();
                    var altID = device["altid"].ToString();
                    var room = long.Parse(device["room"].ToString());
                    var veraRoom = this.VeraRooms.SingleOrDefault(a_item => a_item.ID == room);

                    this.VeraDevices.Add(new VeraDevice
                    {
                        ID = id,
                        ParentID = parentID,
                        Name = name,
                        AltID = altID,
                        VeraRoom = veraRoom
                    });
                }
            }
        }

        public List<VeraRoom> VeraRooms { get; private set; }
        public List<VeraDevice> VeraDevices { get; private set; }
    }
}
