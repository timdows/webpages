using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class ConfigurationValues
    {
        public long Id { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Notes { get; set; }
        public string Setting { get; set; }
        public string Value { get; set; }
    }
}
