using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;

namespace DenSGame.Data.Models
{
    public abstract class SqlBase
    {
        public long ID { get; set; }

        [ScriptIgnore]
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public DateTime? DateDeleted { get; set; }

        [ScriptIgnore]
        [NotMapped]
        public bool IsNew
        {
            get { return this.ID == 0; }
        }
    }
}
