using System.Collections;
using MysensorListener.Controllers.NRF24;

namespace MysensorListener.Models
{
    public class NRF24Data
    {
        public string NodeAddress { get; set; }
        //6 bits
        public BitArray PayloadLength { get; set; }
        //2 bits
        public BitArray Pid { get; set; }
        //1 bit
        public bool NoAck { get; set; }
        public string Payload { get; set; }
        public BitArray PayloadBitArray { get; set; }
        public NRF24Mysensor NRF24Mysensor { get; set; }

        public BitArray PacketCrc { get; set; }

        //int representation of the bitarrays
        public int PayloadLengthNumber => NRF24Helpers.GetNumberFromBitArray(this.PayloadLength);
        public int PidNumber => NRF24Helpers.GetNumberFromBitArray(this.Pid);
    }
}
