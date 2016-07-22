using System;
using System.Collections.Generic;

namespace ExistingDatabase.Models
{
    public partial class Projects
    {
        public Projects()
        {
            Contacts = new HashSet<Contacts>();
            Databases = new HashSet<Databases>();
            Environments = new HashSet<Environments>();
            Files = new HashSet<Files>();
            RemoteDesktops = new HashSet<RemoteDesktops>();
            Webpages = new HashSet<Webpages>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public byte[] Timestamp { get; set; }
        public string Description { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public string Notes { get; set; }
        public string JiraProjectName { get; set; }
        public string JiraProjectFilterUrl { get; set; }
        public string BuglogProjectFilterUrl { get; set; }

        public virtual ICollection<Contacts> Contacts { get; set; }
        public virtual ICollection<Databases> Databases { get; set; }
        public virtual ICollection<Environments> Environments { get; set; }
        public virtual ICollection<Files> Files { get; set; }
        public virtual ICollection<RemoteDesktops> RemoteDesktops { get; set; }
        public virtual ICollection<Webpages> Webpages { get; set; }
    }
}
