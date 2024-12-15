namespace P2PLearningAPI.Models
{
    public class Answer: Post
    {
        public long? AnswerId { get; set; }
        public long? QuestionId { get; set; }
        public Question? Question { get; set; }
        public Answer? AnswerTo { get; set; }
        public bool IsBestAnswer { get; set; } = false;
        public ICollection<Answer> Replies { get; } = new HashSet<Answer>();

        public Answer() { }
        public Answer(
            string Title,
            string Content,
            User PostedBy,
            Question Question
            ) : base(Title, Content, PostedBy)
        {
            this.Question = Question;
            this.QuestionId = Question.Id;
        }

        public Answer(
            string Title,
            string Content,
            User PostedBy,
            Answer AnswerTo
            ) : base(Title, Content, PostedBy)
        {
            this.AnswerTo = AnswerTo;
            this.AnswerId = AnswerTo.Id;
        }
    }
}
