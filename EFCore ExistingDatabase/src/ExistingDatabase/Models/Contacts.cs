using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class Contacts
    {
        public long Id { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string PhoneNumber { get; set; }
        public long? ProjectId { get; set; }

        public virtual Projects Project { get; set; }
    }
}
