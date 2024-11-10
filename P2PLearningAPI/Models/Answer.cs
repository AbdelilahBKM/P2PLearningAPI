namespace P2PLearningAPI.Models
{
    public class Answer
    {
        public long Id { get; set; }
        public Post Post { get; set; } = null!;
        public long PostId { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; }
        public bool IsTheBestAnswer { get; set; } = false;
    }
}
