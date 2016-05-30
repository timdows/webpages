namespace DenSGame.Data.Models
{
    public class Score : SqlBase
    {
        public User User { get; set; }
        public Question Question { get; set; }
        public long Points { get; set; }
    }
}