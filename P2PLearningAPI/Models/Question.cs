namespace P2PLearningAPI.Models
{
    public class Question: Post
    {
        public Discussion Discussion { get; set; } = null!;
        public long DiscussionId { get; set; }
        public ICollection<Answer> Answers { get; } = new List<Answer>();
        public Question(
            string Title, 
            string Content, 
            User PostedBy, 
            Discussion discussion
            ): base(Title, Content, PostedBy) {
            this.Discussion = discussion;
            DiscussionId = discussion.Id;
        }
        
    }
}
