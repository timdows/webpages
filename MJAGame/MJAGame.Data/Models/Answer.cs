using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MJAGame.Data.Models
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
        public bool Correct { get; set; }
        public List<User> ChosenBy { get; set; }
    }
}