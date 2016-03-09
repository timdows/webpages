using System.ComponentModel;

namespace MysensorListener.Settings
{
    public class GeneralSettings
    {
        [Description("The IP address of your Vera3 in your local network")]
        public string VeraIpAddress { get; set; }

        [Description("The IP address of your Mysensors gateway in your local network")]
        public string MysensorsIpAddress { get; set; }
        [Description("The port where the Mysensors gateway is listening on")]
        public int MysensorsPort { get; set; }

        [Description("The serial port (COM) on your machine where the NRF24 sniffer arduino is connected to")]
        public string PortName { get; set; }
        [Description("The buad rate of the serial port of the NRF24 sniffer arduino")]
        public int BaudRate { get; set; }
        [Description("Enables the option to lookup Mysensors node information received via the serial port on the Vera")]
        public bool LookupMysensorsNodeViaVera { get; set; }
    }
}
