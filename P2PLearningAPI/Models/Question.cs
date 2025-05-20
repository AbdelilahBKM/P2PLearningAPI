namespace P2PLearningAPI.Models
{
    public class Question: Post
    {
        public Discussion Discussion { get; set; } = null!;
        public long DiscussionId { get; set; }
        public ICollection<Answer> Answers { get; } = new List<Answer>();
        public bool isAnswered { get; set; } = false;
        public Simularity? Simularity { get; set; }
        public Question() { }
        public Question(
            string Title,
            string Content,
            string PostedBy,
            long discussionId
            ): base(Title, Content, PostedBy) {
            this.DiscussionId = discussionId;
        }
    }
}
