namespace P2PLearningAPI.Models
{
    public class Post
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsUpdated { get; set; } = false;
        public long Reputation { get; set; } = 0;
        public DateTime PostedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; }
        public long UserID { get; set; }
        public User PostedBy { get; set; } = null!;
        public bool IsClosed { get; set; } = false;
        public ICollection<Vote> Votes { get; } = new HashSet<Vote>();
    }
}
