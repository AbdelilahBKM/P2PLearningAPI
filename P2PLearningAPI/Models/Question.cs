namespace P2PLearningAPI.Models
{
    public class Question
    {
        public long Id { get; set; }
        public Post Post { get; set; } = null!;
        public long PostId { get; set; }
        public Discussion Discussion { get; set; } = null!;
        public long DiscussionId { get; set; }
        public ICollection<Answer> Answers { get; } = new List<Answer>();
        
    }
}
