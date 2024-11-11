namespace P2PLearningAPI.Models
{
    public class Question: Post
    {
        public Discussion Discussion { get; set; } = null!;
        public long DiscussionId { get; set; }
        public ICollection<Answer> Answers { get; } = new List<Answer>();
        
    }
}
