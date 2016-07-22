using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class RemoteDesktops
    {
        public long Id { get; set; }
        public string IpAddress { get; set; }
        public long? ProjectId { get; set; }
        public byte[] Timestamp { get; set; }
        public string Username { get; set; }
        public string Description { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Notes { get; set; }

        public virtual Projects Project { get; set; }
    }
}
