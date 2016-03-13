using System;
using System.Collections;
using System.Globalization;
using MysensorListener.Controllers.NRF24;

namespace MysensorListener.Models
{
    public class NRF24Mysensor
    {
        public byte Last { get; set; }
        public byte Sender { get; set; }
        public byte Destination { get; set; }
        //5 bits
        public BitArray Length { get; set; }
        //3 bits
        public BitArray Version { get; set; }
        //3 bits
        public BitArray DataType { get; set; }
        //1 bit
        public bool IsAck { get; set; }
        //1 bit
        public bool ReqAck { get; set; }
        //3 bits
        public BitArray CommandType { get; set; }
        public byte Type { get; set; }
        public byte Sensor { get; set; }

        public BitArray PayloadBitArray { get; set; }

        public string Payload
        {
            get
            {
                byte[] bytes;
                switch ((MysensorsEnums.PayloadType) this.DataTypeNumber)
                {
                    case MysensorsEnums.PayloadType.FLOAT32: //Float - Little Endian (DCBA)
                        bytes = NRF24Helpers.GetBytesFromBitArray(this.PayloadBitArray);
                        if (bytes.Length < 4)
                            return "FLOAT32 error";
                        var floatResult = BitConverter.ToSingle(bytes, 0);
                        return floatResult.ToString(CultureInfo.InvariantCulture);
                    case MysensorsEnums.PayloadType.BYTE: //UINT16 - Little Endian (BA)
                        bytes = new byte[2];
                        bytes[0] = NRF24Helpers.GetByteFromBitArray(this.PayloadBitArray, 0);
                        bytes[1] = 0x00;
                        var shortResult = BitConverter.ToInt16(bytes, 0);
                        return shortResult.ToString(CultureInfo.InvariantCulture);
                    case MysensorsEnums.PayloadType.INT16:
                        bytes = NRF24Helpers.GetBytesFromBitArray(this.PayloadBitArray);
                        if (bytes.Length < 2)
                            return "INT16 error";
                        var int16Result = BitConverter.ToInt16(bytes, 0);
                        return int16Result.ToString(CultureInfo.InvariantCulture);
                    default:
                        //return NRF24Helpers.BitArrayToString(this.PayloadBitArray);
                        return NRF24Helpers.GetHexStringFromBitArray(this.PayloadBitArray);
                }
            }
        }

        public string PayloadHexString
        {
            get
            {
                var hexString = string.Empty;
                for (var i = 0; i <= this.PayloadBitArray.Length - 8; i += 8)
                {
                    var bitArray = NRF24Helpers.GetPartOfBitArray(this.PayloadBitArray, i, 8);
                    hexString += NRF24Helpers.GetHexStringFromBitArray(bitArray);
                }
                return hexString;
            }
        }

        //int representation of the bitarrays
        public int LengthNumber => NRF24Helpers.GetNumberFromBitArray(this.Length);
        public int VersionNumber => NRF24Helpers.GetNumberFromBitArray(this.Version);
        public int DataTypeNumber => NRF24Helpers.GetNumberFromBitArray(this.DataType);
        public int CommandTypeNumber => NRF24Helpers.GetNumberFromBitArray(this.CommandType);

        //string representation of the lookups
        public string VersionString => ((MysensorsEnums.ProtocolVersion) this.VersionNumber).ToString();
        public string DataTypeString => ((MysensorsEnums.PayloadType) this.DataTypeNumber).ToString();
        public string CommandTypeString => ((MysensorsEnums.MessageTypeDefinition) this.CommandTypeNumber).ToString();

        public string TypeString
        {
            get
            {
                switch ((MysensorsEnums.MessageTypeDefinition) this.CommandTypeNumber)
                {
                    case MysensorsEnums.MessageTypeDefinition.Presentation:
                        return ((MysensorsEnums.PresentationSubType) this.Type).ToString();
                    case MysensorsEnums.MessageTypeDefinition.Req:
                    case MysensorsEnums.MessageTypeDefinition.Set:
                        return ((MysensorsEnums.SetReqSubType) this.Type).ToString();
                    case MysensorsEnums.MessageTypeDefinition.Internal:
                        return ((MysensorsEnums.InternalSubType) this.Type).ToString();
                    default:
                        return "unknown";
                }
            }
        }

        //Objects that identify the device via the Vera3 environment
        //Sender has always childID 255
        public VeraDevice SenderVeraDevice { get; set; }
        //Destination has always childID 255
        public VeraDevice DestinationVeraDevice { get; set; }
        //Specifies nodeID and childID
        public VeraDevice SensorVeraDevice { get; set; }
    }
}
