using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class WebpageBrowsers
    {
        public WebpageBrowsers()
        {
            Webpages = new HashSet<Webpages>();
        }

        public long Id { get; set; }
        public string BrowserPath { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }

        public virtual ICollection<Webpages> Webpages { get; set; }
    }
}
