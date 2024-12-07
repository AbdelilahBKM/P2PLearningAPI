namespace P2PLearningAPI.Models
{
    public class Answer: Post
    {
        public long PostId { get; set; }
        public long QuestionId { get; set; }
        public Question Question { get; set; }
        public bool IsBestAnswer { get; set; } = false;
        public Answer(
            string Title,
            string Content,
            User PostedBy,
            Question Question
            ): base(Title, Content, PostedBy) {
            this.Question = Question;
            this.QuestionId = Question.Id;
        }
    }
}
