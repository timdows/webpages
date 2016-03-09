using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using MysensorListener.Models;
using MysensorListener.Settings;

namespace MysensorListener.Controllers.NRF24
{
    public class NRF24Hub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly GeneralSettings _generalSettings;
        private readonly NRF24State _nrf24State;
        private readonly VeraSettings _veraSettings;

        private string _rawData;

        public NRF24Hub(IConnectionManager connectionManager,
            GeneralSettings generalSettings,
            NRF24State nrf24State,
            VeraSettings veraSettings)
        {
            _connectionManager = connectionManager;
            _generalSettings = generalSettings;
            _nrf24State = nrf24State;
            _veraSettings = veraSettings;

            _rawData = string.Empty;
        }

        private void SendObject(NRF24Structure nrf24Structure)
        {
            var context = _connectionManager.GetHubContext<NRF24Hub>();
            context.Clients.All.broadcastObject(nrf24Structure);
        }

        public async void StartSerialClient()
        {
            using (var serialPort = new SerialPort(
                _generalSettings.PortName,
                _generalSettings.BaudRate,
                Parity.None,
                8,
                StopBits.One))
            {
                serialPort.Open();

                while (true)
                {
                    // Wait for the asynchronous action to complete
                    await ReadDataAsync(serialPort);
                }
            }
        }

        private async Task ReadDataAsync(SerialPort serialPort)
        {
            var buffer = new byte[4096];
            Task<int> readStringTask = serialPort.BaseStream.ReadAsync(buffer, 0, 4096);

            var bytesRead = await readStringTask;
            _rawData += Encoding.ASCII.GetString(buffer, 0, bytesRead);

            // Check if a newline has received, indicating that a complete message should have read
            var lastIndexOfPrintln = _rawData.LastIndexOf("\r\n", StringComparison.Ordinal);

            if (lastIndexOfPrintln == -1)
                return;

            var completeMessages = _rawData.Substring(0, lastIndexOfPrintln);
            _rawData = _rawData.Remove(0, lastIndexOfPrintln);

            // Split message if more than one have been received
            var messages = completeMessages.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var message in messages)
            {
                _nrf24State.CountOfReceivedMessages++;

                // Every message has three parts splitted by a space
                // - record length & message type
                // - serial header
                // - packet data
                var parts = message.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Count() != 3)
                    continue;

                var nrf24Structure = new NRF24Structure
                {
                    DateTime = DateTime.Now,
                    TypeAndLength = parts[0],
                    Header = parts[1],
                    Data = parts[2]
                };

                // Create the NRF24Header part
                nrf24Structure.NRF24Header = NRF24Hub.CreateNRF24Header(nrf24Structure);

                // Create the NRF24Data part
                nrf24Structure.NRF24Data = CreateNRF24Data(nrf24Structure);

                

                

                SendObject(nrf24Structure);
            }
        }

        private static NRF24Header CreateNRF24Header(NRF24Structure nrf24Structure)
        {
            return (nrf24Structure.Header.Length == 18)
                ? new NRF24Header
                {
                    Timestamp = nrf24Structure.Header.Substring(0, 8),
                    PacketsLost = nrf24Structure.Header.Substring(8, 2),
                    Address = nrf24Structure.Header.Substring(10, 8)
                }
                : null;
        }

        private NRF24Data CreateNRF24Data(NRF24Structure nrf24Structure)
        {
            if (nrf24Structure.NRF24Header == null || nrf24Structure.Data.Length < 5)
                return null;

            // Set the NRF24 data part
            var nrf24Data = new NRF24Data
            {
                NodeAddress = $"{nrf24Structure.NRF24Header.Address}{nrf24Structure.Data.Substring(0, 2)}"
            };

            var controlFieldString = nrf24Structure.Data.Substring(2, 4);
            var controlFieldBitArray = NRF24Helpers.GetBitArrayFromHexString(controlFieldString);

            nrf24Data.PayloadLength = NRF24Helpers.GetPartOfBitArray(controlFieldBitArray, 0, 6);
            nrf24Data.Pid = NRF24Helpers.GetPartOfBitArray(controlFieldBitArray, 6, 2);
            nrf24Data.NoAck = controlFieldBitArray[9];

            nrf24Data.Payload = nrf24Structure.Data.Substring(4, nrf24Structure.Data.Length - 4);
            var payloadFieldBitArray = NRF24Helpers.GetBitArrayFromHexString(nrf24Data.Payload);

            // For last byte only the MSbit has value; rest will be cleared
            payloadFieldBitArray = NRF24Helpers.GetPartOfBitArray(payloadFieldBitArray, 0, payloadFieldBitArray.Length - 7);

            // Get the packet crc (the crc is located in last two bytes of the packet)
            var crcOffset = payloadFieldBitArray.Length - 16;
            nrf24Data.PacketCrc = NRF24Helpers.GetPartOfBitArray(payloadFieldBitArray, crcOffset, 16);

            // Remove the crc bits from the bitarray before it is processed further
            nrf24Data.PayloadBitArray = NRF24Helpers.GetPartOfBitArray(payloadFieldBitArray, 0, payloadFieldBitArray.Length - 16);

            //TODO what about the string payload??

            // ****
            // Calculate the crc ourselfs

            // Get the payload in bits, as the bitArray is already stripped of all unneeded stuff, the length is the amount of bits
            //var payloadLengthBits = payloadFieldBitArray.Length;
            //var packetLengthBits = 40 + 9 + payloadLengthBits;
            // As the crc is calculated over the address, packet control field and payload, attach the two fields
            //var combinedString = $"{this.NRF24Header.Address}{this.Data}";
            //var combinedString = $"{this.Data}";

            // Create the NRF24Mysensor part
            nrf24Data.NRF24Mysensor = CreateNRF24Mysensor(nrf24Data);

            return nrf24Data;
        }

        private NRF24Mysensor CreateNRF24Mysensor(NRF24Data nrf24Data)
        {
            // Check if the bitArray contains enough information to be processed
            if (nrf24Data.PayloadBitArray.Length < 57)
                return null;

            // We should start at 1 as the string is not byte aligned
            var bitOffset = 1;

            // Set the MySensors data part
            var nrf24Mysensor = new NRF24Mysensor();

            nrf24Mysensor.Last = NRF24Helpers.GetByteFromBitArray(nrf24Data.PayloadBitArray, bitOffset);
            bitOffset += 8;

            nrf24Mysensor.Sender = NRF24Helpers.GetByteFromBitArray(nrf24Data.PayloadBitArray, bitOffset);
            bitOffset += 8;

            nrf24Mysensor.Destination = NRF24Helpers.GetByteFromBitArray(nrf24Data.PayloadBitArray,
                bitOffset);
            bitOffset += 8;

            nrf24Mysensor.Length = NRF24Helpers.GetPartOfBitArray(nrf24Data.PayloadBitArray, bitOffset, 5);
            bitOffset += 5;

            nrf24Mysensor.Version = NRF24Helpers.GetPartOfBitArray(nrf24Data.PayloadBitArray, bitOffset, 3);
            bitOffset += 3;

            nrf24Mysensor.DataType = NRF24Helpers.GetPartOfBitArray(nrf24Data.PayloadBitArray, bitOffset, 3);
            bitOffset += 3;

            nrf24Mysensor.IsAck = nrf24Data.PayloadBitArray[bitOffset];
            bitOffset++;

            nrf24Mysensor.ReqAck = nrf24Data.PayloadBitArray[bitOffset];
            bitOffset++;

            nrf24Mysensor.CommandType = NRF24Helpers.GetPartOfBitArray(nrf24Data.PayloadBitArray, bitOffset, 3);
            bitOffset += 3;

            nrf24Mysensor.Type = NRF24Helpers.GetByteFromBitArray(nrf24Data.PayloadBitArray, bitOffset);
            bitOffset += 8;

            nrf24Mysensor.Sensor = NRF24Helpers.GetByteFromBitArray(nrf24Data.PayloadBitArray, bitOffset);
            bitOffset += 8;

            nrf24Mysensor.PayloadBitArray = NRF24Helpers.GetPartOfBitArray(
                nrf24Data.PayloadBitArray,
                bitOffset,
                nrf24Data.PayloadBitArray.Length - bitOffset);

            // Find, is enabled, the VeraDevices corresponding to the package
            if (_generalSettings.LookupMysensorsNodeViaVera)
            {
                // Check if it is the gateway that sends the message
                if (nrf24Mysensor.Sender == 0)
                {
                    nrf24Mysensor.SenderVeraDevice = _veraSettings.VeraDevices
                        .SingleOrDefault(a_item => a_item.IsGateway);
                }
                else
                {
                    nrf24Mysensor.SenderVeraDevice = _veraSettings.VeraDevices
                        .SingleOrDefault(
                            a_item => a_item.VeraDeviceAltID != null &&
                                      a_item.VeraDeviceAltID.NodeID == nrf24Mysensor.Sender &&
                                      a_item.VeraDeviceAltID.ChildID == 255);
                }

                // Check if it is the gateway that receives the message
                if (nrf24Mysensor.Destination == 0)
                {
                    nrf24Mysensor.DestinationVeraDevice = _veraSettings.VeraDevices
                        .SingleOrDefault(a_item => a_item.IsGateway);
                }
                else
                {
                    nrf24Mysensor.DestinationVeraDevice = _veraSettings.VeraDevices
                        .Where(a_item => a_item.VeraDeviceAltID != null)
                            .SingleOrDefault(a_item =>
                                a_item.VeraDeviceAltID.NodeID == nrf24Mysensor.Destination &&
                                a_item.VeraDeviceAltID.ChildID == 255);
                }

                nrf24Mysensor.SensorVeraDevice = _veraSettings.VeraDevices
                    .Where(a_item => a_item.VeraDeviceAltID != null)
                        .SingleOrDefault(
                            a_item => a_item.VeraDeviceAltID.NodeID == nrf24Mysensor.Sender &&
                                      a_item.VeraDeviceAltID.ChildID == nrf24Mysensor.Sensor);
            }

            return nrf24Mysensor;
        }
    }
}