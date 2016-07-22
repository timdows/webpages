using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class HourLogEntries
    {
        public long Id { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public long? HourLogId { get; set; }
        public string Notes { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? StopDateTime { get; set; }

        public virtual HourLogs HourLog { get; set; }
    }
}
