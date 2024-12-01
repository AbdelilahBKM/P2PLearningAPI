namespace P2PLearningAPI.Models
{
    public class Answer: Post
    {
        public long PostId { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; }
        public bool IsBestAnswer { get; set; } = false;
    }
}
