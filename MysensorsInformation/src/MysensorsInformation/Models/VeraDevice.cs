using System.Linq;

namespace MysensorListener.Models
{
    public class VeraDevice
    {
        public long ID { get; set; }
        public long ParentID { get; set; }
        public string Name { get; set; }
        public string AltID { get; set; }
        public VeraRoom VeraRoom { get; set; }
        public bool IsGateway { get; set; }

        public VeraDeviceAltID VeraDeviceAltID
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.AltID))
                    return null;

                var split = this.AltID.Split(';');
                if (split.Count() != 2)
                    return null;

                return new VeraDeviceAltID
                {
                    NodeID = long.Parse(split[0]),
                    ChildID = long.Parse(split[1])
                };
            }
        }
    }
}
