using System;

namespace MysensorListener.Models
{
    public class NRF24Structure
    {
        // The datetime it was received
        public DateTime DateTime { get; set; }
        public string TypeAndLength { get; set; }
        public string Header { get; set; }
        public string Data { get; set; }

        public NRF24Header NRF24Header { get; set; }
        public NRF24Data NRF24Data { get; set; }
    }
}