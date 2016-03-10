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
        [Description("The RF channel, range between 0 and 127")]
        public int RfChannel { get; set; }
        [Description("The data rate, 0 = 1Mb/s, 1=2Mb/s, 2=250Kb/s")]
        public int DataRate { get; set; }
        [Description("Promiscuous address length in bytes between 3 and 5")]
        public int AddressLength { get; set; }
        [Description("The base address to listen on")]
        public string BaseAddress { get; set; }
        [Description("The package CRC length in bytes between 0 and 2")]
        public int CrcLength { get; set; }
        [Description("The maximum payload size in bytes between 0 and 32")]
        public int MaximumPayloadSize { get; set; }
        [Description("Enables the option to lookup Mysensors node information received via the serial port on the Vera")]
        public bool LookupMysensorsNodeViaVera { get; set; }
    }
}
