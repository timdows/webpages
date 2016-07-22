using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class EnvironmentDatabase
    {
        public long EnvironmentId { get; set; }
        public long DatabaseId { get; set; }

        public virtual Databases Database { get; set; }
        public virtual Environments Environment { get; set; }
    }
}
