using P2PLearningAPI.Interfaces;

namespace P2PLearningAPI.Models
{
    public class Answer: Post
    {
        public long? AnswerId { get; set; }
        public long? QuestionId { get; set; }
        public Question? Question { get; set; }
        public Answer? AnswerTo { get; set; }
        public bool IsBestAnswer { get; set; } = false;
        public bool IsAIGenerated { get; set; } = false;
        public ICollection<Answer> Replies { get; } = new HashSet<Answer>();

        public Answer() { }
        public Answer(
            string Title,
            string Content,
            string PostedBy,
            long id,
            PostType postType,
            bool isAIGenerated = false
            ) : base(Title, Content, PostedBy)
        {
            if (postType == PostType.Answer)
            {
                this.QuestionId = id;
            }
            else if (postType == PostType.Reply)
            {
                this.AnswerId = id;
            }
        }
    }
}
