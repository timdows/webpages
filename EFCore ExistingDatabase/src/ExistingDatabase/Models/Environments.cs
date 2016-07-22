using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class Environments
    {
        public Environments()
        {
            EnvironmentDatabase = new HashSet<EnvironmentDatabase>();
            EnvironmentWebpage = new HashSet<EnvironmentWebpage>();
        }

        public long Id { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public long? ProjectId { get; set; }

        public virtual ICollection<EnvironmentDatabase> EnvironmentDatabase { get; set; }
        public virtual ICollection<EnvironmentWebpage> EnvironmentWebpage { get; set; }
        public virtual Projects Project { get; set; }
    }
}
