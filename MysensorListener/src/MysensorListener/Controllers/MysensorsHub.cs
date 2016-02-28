using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using MysensorListener.Models;
using MysensorListener.Settings;

namespace MysensorListener.Controllers
{
    public class MysensorsHub : Hub
    {
        private readonly IConnectionManager _connectionManager;
        private readonly GeneralSettings _generalSettings;
        private readonly VeraSettings _veraSettings;

        public MysensorsHub(IConnectionManager connectionManager,
            VeraSettings veraSettings,
            GeneralSettings generalSettings)
        {
            _connectionManager = connectionManager;
            _veraSettings = veraSettings;
            _generalSettings = generalSettings;
        }

        private void Send(DateTime datetime, string message)
        {
            var context = _connectionManager.GetHubContext<MysensorsHub>();
            context.Clients.All.broadcastMessage(datetime.ToString("HH:mm:ss.fff"), message);
        }

        private void SendObject(DateTime datetime, MysensorsMessageStructure mysensorsMessageStructure)
        {
            var context = _connectionManager.GetHubContext<MysensorsHub>();
            context.Clients.All.broadcastObject(datetime.ToString("HH:mm:ss.fff"), mysensorsMessageStructure);
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
                    var rawReceivedString = Encoding.ASCII.GetString(data, 0, bytes);

                    if (string.IsNullOrWhiteSpace(rawReceivedString))
                        continue;

                    if (!rawReceivedString.Contains((char) 10))
                        continue;

                    // Split message if more than one have been received
                    var messages = rawReceivedString.Split((char)10)
                        .Where(a_item => !string.IsNullOrWhiteSpace(a_item));

                    foreach (var message in messages)
                    {
                        var splitted = message.Split(';');
                        if (splitted.Count() != 6)
                        {
                            // Something went wrong, send out the complete message
                            Send(DateTime.Now, message);
                            continue;
                        }

                        var messageStructure = new MysensorsMessageStructure
                        {
                            NodeID = int.Parse(splitted[0]),
                            ChildSensorID = int.Parse(splitted[1]),
                            MessageType = (MysensorsMessageStructure.MessageTypeDefinition)int.Parse(splitted[2]),
                            Ack = int.Parse(splitted[3]) == 1,
                            Subtype = int.Parse(splitted[4]),
                            Payload = splitted[5],
                        };
                        messageStructure.VeraDevice = _veraSettings.VeraDevices
                            .Where(a_item => a_item.VeraDeviceAltID != null)
                            .SingleOrDefault(
                                a_item => a_item.VeraDeviceAltID.NodeID == messageStructure.NodeID &&
                                            a_item.VeraDeviceAltID.ChildID == messageStructure.ChildSensorID);

                        SendObject(DateTime.Now, messageStructure);
                    }
                }
            }
        }
    }
}
