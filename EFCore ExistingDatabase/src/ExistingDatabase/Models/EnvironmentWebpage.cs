using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class EnvironmentWebpage
    {
        public long EnvironmentId { get; set; }
        public long WebpageId { get; set; }

        public virtual Environments Environment { get; set; }
        public virtual Webpages Webpage { get; set; }
    }
}
