using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class WebpageTypes
    {
        public WebpageTypes()
        {
            Webpages = new HashSet<Webpages>();
        }

        public long Id { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Webpages> Webpages { get; set; }
    }
}
