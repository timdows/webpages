using Microsoft.AspNet.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HouseDB.ClientModels
{
    public class SevenSegmentClientModel
    {
        private const string Server = "http://10.0.0.15";
        private const string RequestString = "/port_3480/data_request?id=status&output_format=json&DeviceNum=";
        private const int WattChannel = 38;

        public string Watt { get; set; }
        public string LastWeekTotal { get; set; }
        public string ThisWeekTotal { get; set; }
        public string LastMonthTotal { get; set; }
        public string ThisMonthTotal { get; set; }

        public async Task<JsonResult> Load()
        {
            var url = string.Format("{0}{1}{2}",
                SevenSegmentClientModel.Server,
                SevenSegmentClientModel.RequestString,
                SevenSegmentClientModel.WattChannel);

            using (var webClient = new HttpClient())
            {
                var result = await webClient.GetStringAsync(url);
                var json = JObject.Parse(result);
                this.Watt = json["Device_Num_38"]["states"][0]["value"].ToString();
            }

            return new JsonResult(true);
        }
    }
}
