using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using MysensorListener.Models;
using MysensorListener.Settings;

namespace MysensorListener.Controllers.Mysensors
{
    public class MysensorsHub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly GeneralSettings _generalSettings;
        private readonly VeraSettings _veraSettings;
        private readonly MysensorsState _mysensorsState;

        private string _rawData;

        public MysensorsHub(IConnectionManager connectionManager,
            VeraSettings veraSettings,
            GeneralSettings generalSettings,
            MysensorsState mysensorsState)
        {
            _connectionManager = connectionManager;
            _veraSettings = veraSettings;
            _generalSettings = generalSettings;
            _mysensorsState = mysensorsState;

            _rawData = string.Empty;
        }

        private void Send(DateTime datetime, string message)
        {
            var context = _connectionManager.GetHubContext<MysensorsHub>();
            context.Clients.All.broadcastMessage(datetime.ToString("HH:mm:ss.fff"), message);
        }

        private void SendObject(MysensorsStructure mysensorsStructure)
        {
            var context = _connectionManager.GetHubContext<MysensorsHub>();
            context.Clients.All.broadcastObject(mysensorsStructure);
        }

        public void StartTelnetClient()
        {
            using (var client = new TcpClient(_generalSettings.MysensorsIpAddress, _generalSettings.MysensorsPort))
            {
                var stream = client.GetStream();
                var data = new byte[1024];

                while (true)
                {
                    var bytes = stream.Read(data, 0, data.Length);
                    _rawData += Encoding.ASCII.GetString(data, 0, bytes);

                    // Check if a newline has received, indicating that a complete message should have read
                    var lastIndexOfPrintln = _rawData.LastIndexOf((char)10);

                    if (lastIndexOfPrintln == -1)
                        return;

                    var completeMessages = _rawData.Substring(0, lastIndexOfPrintln);
                    _rawData = _rawData.Remove(0, lastIndexOfPrintln);

                    // Split message if more than one have been received
                    var messages = completeMessages.Split((char)10).Where(a_item => !string.IsNullOrWhiteSpace(a_item));

                    foreach (var message in messages)
                    {
                        _mysensorsState.CountOfReceivedMessages++;

                        var splitted = message.Split(';');
                        if (splitted.Count() != 6)
                        {
                            // Something went wrong, send out the complete message
                            Send(DateTime.Now, message);
                            continue;
                        }

                        var messageStructure = new MysensorsStructure
                        {
                            DateTime = DateTime.Now,
                            NodeID = int.Parse(splitted[0]),
                            ChildSensorID = int.Parse(splitted[1]),
                            MessageType = (MysensorsEnums.MessageTypeDefinition)int.Parse(splitted[2]),
                            Ack = int.Parse(splitted[3]) == 1,
                            Subtype = int.Parse(splitted[4]),
                            Payload = splitted[5],
                        };

                        // Find the associated veradevice
                        messageStructure.VeraDevice = _veraSettings.VeraDevices
                            .Where(a_item => a_item.VeraDeviceAltID != null)
                            .SingleOrDefault(
                                a_item => a_item.VeraDeviceAltID.NodeID == messageStructure.NodeID &&
                                            a_item.VeraDeviceAltID.ChildID == messageStructure.ChildSensorID);

                        SendObject(messageStructure);
                    }
                }
            }
        }
    }
}
