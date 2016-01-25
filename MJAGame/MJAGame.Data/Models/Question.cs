using System.ComponentModel.DataAnnotations.Schema;

namespace MJAGame.Data.Models
{
    public class Question : SqlBase
    {
        public enum QuestionStatus
        {
            SubmitAnswer = 1,
            SelectLies = 2,
            ShowScores = 3,
            Ended = 4
        }

        public Subject Subject { get; set; }
        public string QuestionString { get; set; }
        public int Status { get; set; }
        public User SubmittedUser { get; set; }

        [NotMapped]
        public long AmountOfAnswers { get; set; }
        [NotMapped]
        public long AmountOfLies { get; set; }
    }
}