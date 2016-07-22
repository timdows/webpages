using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class Files
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long? ProjectId { get; set; }
        public byte[] Timestamp { get; set; }
        public int Type { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Notes { get; set; }

        public virtual Projects Project { get; set; }
    }
}
