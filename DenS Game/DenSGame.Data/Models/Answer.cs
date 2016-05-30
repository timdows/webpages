using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DenSGame.Data.Models
{
    public class Answer : SqlBase
    {
        public Answer()
        {
            this.ChosenBy = new List<User>();
        }

        public Question Question { get; set; }
        public User User { get; set; }
        public string AnswerString { get; set; }
        public bool PickedByDennis { get; set; }
        public bool PickedBySandra { get; set; }
        public List<User> ChosenBy { get; set; }

        [NotMapped]
        public bool UserPickedDSAnswer { get; set; }
    }
}