using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class Databases
    {
        public Databases()
        {
            EnvironmentDatabase = new HashSet<EnvironmentDatabase>();
        }

        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public string Server { get; set; }
        public byte[] Timestamp { get; set; }
        public int AuthenticationType { get; set; }
        public string DatabaseName { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string User { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<EnvironmentDatabase> EnvironmentDatabase { get; set; }
        public virtual Projects Project { get; set; }
    }
}
