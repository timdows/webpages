using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class Webpages
    {
        public Webpages()
        {
            EnvironmentWebpage = new HashSet<EnvironmentWebpage>();
        }

        public long Id { get; set; }
        public long? ProjectId { get; set; }
        public byte[] Timestamp { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Notes { get; set; }
        public long? WebpageTypeId { get; set; }
        public long? WebpageBrowserId { get; set; }

        public virtual ICollection<EnvironmentWebpage> EnvironmentWebpage { get; set; }
        public virtual Projects Project { get; set; }
        public virtual WebpageBrowsers WebpageBrowser { get; set; }
        public virtual WebpageTypes WebpageType { get; set; }
    }
}
