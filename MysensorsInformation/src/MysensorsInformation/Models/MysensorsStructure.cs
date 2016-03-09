using System;

namespace MysensorListener.Models
{
    public class MysensorsStructure
    {
        // The datetime it was received
        public DateTime DateTime { get; set; }
        public int NodeID { get; set; }
        public int ChildSensorID { get; set; }
        public MysensorsEnums.MessageTypeDefinition MessageType { get; set; }
        public string MessageTypeString => this.MessageType.ToString();
        public bool Ack { get; set; }
        public int Subtype { get; set; }

        public string SubtypeString
        {
            get
            {
                switch (this.MessageType)
                {
                    case MysensorsEnums.MessageTypeDefinition.Presentation:
                        return ((MysensorsEnums.PresentationSubType) this.Subtype).ToString();
                    case MysensorsEnums.MessageTypeDefinition.Req:
                    case MysensorsEnums.MessageTypeDefinition.Set:
                        return ((MysensorsEnums.SetReqSubType)this.Subtype).ToString();
                    case MysensorsEnums.MessageTypeDefinition.Internal:
                        return ((MysensorsEnums.InternalSubType)this.Subtype).ToString();
                    default:
                        return "unknown";
                }
            }
        }

        public string Payload { get; set; }
        public VeraDevice VeraDevice { get; set; }
    }
}
