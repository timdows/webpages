using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class HourLogs
    {
        public HourLogs()
        {
            HourLogEntries = new HashSet<HourLogEntries>();
        }

        public long Id { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<HourLogEntries> HourLogEntries { get; set; }
    }
}
