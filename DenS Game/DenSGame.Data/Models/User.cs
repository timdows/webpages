namespace DenSGame.Data.Models
{
    public class User : SqlBase
    {
        public string Name { get; set; }
        public bool IsRoot { get; set; }
        public bool IsDennis { get; set; }
        public bool IsSandra { get; set; }
        public bool IsQuizmaster { get; set; }
    }
}